<Query Kind="SQL" />

/* Taken from the following links:
	- https://github.com/microsoft/sql-server-samples/blob/master/samples/features/in-memory-database/in-memory-oltp/t-sql-scripts/enable-in-memory-oltp.sql
	- https://docs.microsoft.com/en-us/sql/relational-databases/in-memory-oltp/overview-and-usage-scenarios?view=sql-server-ver15
*/
-- The below scipt enables the use of In-Memory OLTP in the current database, 
--   provided it is supported in the edition / pricing tier of the database.
-- It does the following:
-- 1. Validate that In-Memory OLTP is supported. 
-- 2. In SQL Server, it will add a MEMORY_OPTIMIZED_DATA filegroup to the database
--    and create a container within the filegroup in the default data folder.
-- 3. Change the database compatibility level to 130 (needed for parallel queries
--    and auto-update of statistics).
-- 4. Enables the database option MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT to avoid the 
--    need to use the WITH (SNAPSHOT) hint for ad hoc queries accessing memory-optimized
--    tables.
-- 5. Creates memory optimized tables, types and procedures
-- 6. Include example executions of the created objects
-- 7. Cleanup objects
--
-- Applies To: SQL Server 2016 (or higher); Azure SQL Database
-- Author: Microsoft
-- Last Updated: 2016-05-02

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @Cleanup bit = 1;

-- 1. validate that In-Memory OLTP is supported
IF SERVERPROPERTY(N'IsXTPSupported') = 0 
BEGIN                                    
    PRINT N'Error: In-Memory OLTP is not supported for this server edition or database pricing tier.';
END 
IF DB_ID() < 5
BEGIN                                    
    PRINT N'Error: In-Memory OLTP is not supported in system databases. Connect to a user database.';
END 
ELSE 
BEGIN 
	BEGIN TRY;
-- 2. add MEMORY_OPTIMIZED_DATA filegroup when not using Azure SQL DB
	IF SERVERPROPERTY('EngineEdition') != 5 
	BEGIN
		DECLARE @SQLDataFolder nvarchar(max) = cast(SERVERPROPERTY('InstanceDefaultDataPath') as nvarchar(max))
		DECLARE @MODName nvarchar(max) = DB_NAME() + N'_mod';
		DECLARE @MemoryOptimizedFilegroupFolder nvarchar(max) = @SQLDataFolder + @MODName;

		DECLARE @SQL nvarchar(max) = N'';

		-- add filegroup
		IF NOT EXISTS (SELECT 1 FROM sys.filegroups WHERE type = N'FX')
		BEGIN
			SET @SQL = N'ALTER DATABASE CURRENT ADD FILEGROUP ' + 
					QUOTENAME(@MODName) + N' CONTAINS MEMORY_OPTIMIZED_DATA;';
			EXECUTE (@SQL);
		END;

		-- add container in the filegroup
		IF NOT EXISTS 
		(
			SELECT * FROM sys.database_files WHERE data_space_id IN 
			(SELECT data_space_id FROM sys.filegroups WHERE type = N'FX')
		)
		BEGIN
			SET @SQL = N'ALTER DATABASE CURRENT ADD FILE (name = N''' + @MODName + ''', filename = '''
						+ @MemoryOptimizedFilegroupFolder + N''') TO FILEGROUP ' + QUOTENAME(@MODName);
			EXECUTE (@SQL);
		END
	END

-- 3. set compat level to 130 if it is lower
	IF (SELECT compatibility_level FROM sys.databases WHERE database_id=DB_ID()) < 130
		ALTER DATABASE CURRENT SET COMPATIBILITY_LEVEL = 130 

-- 4. enable MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT for the database
	ALTER DATABASE CURRENT SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT = ON;


    END TRY
    BEGIN CATCH
        PRINT N'Error enabling In-Memory OLTP';
		IF XACT_STATE() != 0
			ROLLBACK;
        THROW;
    END CATCH;
END;
GO

--5. Create sample tables, types and procedures
-- memory-optimized table
CREATE TABLE dbo.table1
( c1 INT IDENTITY PRIMARY KEY NONCLUSTERED,
  c2 NVARCHAR(MAX))
WITH (MEMORY_OPTIMIZED=ON)
GO
-- non-durable table
CREATE TABLE dbo.temp_table1
( c1 INT IDENTITY PRIMARY KEY NONCLUSTERED,
  c2 NVARCHAR(MAX))
WITH (MEMORY_OPTIMIZED=ON,
      DURABILITY=SCHEMA_ONLY)
GO
-- memory-optimized table type
CREATE TYPE dbo.tt_table1 AS TABLE
( c1 INT IDENTITY,
  c2 NVARCHAR(MAX),
  is_transient BIT NOT NULL DEFAULT (0),
  INDEX ix_c1 HASH (c1) WITH (BUCKET_COUNT=1024))
WITH (MEMORY_OPTIMIZED=ON)
GO
-- natively compiled stored procedure
CREATE PROCEDURE dbo.usp_ingest_table1
  @table1 dbo.tt_table1 READONLY
WITH NATIVE_COMPILATION, SCHEMABINDING
AS
BEGIN ATOMIC
    WITH (TRANSACTION ISOLATION LEVEL=SNAPSHOT,
          LANGUAGE=N'us_english')

  DECLARE @i INT = 1

  WHILE @i > 0
  BEGIN
    INSERT dbo.table1
    SELECT c2
    FROM @table1
    WHERE c1 = @i AND is_transient=0

    IF @@ROWCOUNT > 0
      SET @i += 1
    ELSE
    BEGIN
      INSERT dbo.temp_table1
      SELECT c2
      FROM @table1
      WHERE c1 = @i AND is_transient=1

      IF @@ROWCOUNT > 0
        SET @i += 1
      ELSE
        SET @i = 0
    END
  END

END
GO
--6. Sample execution of objects
DECLARE @table1 dbo.tt_table1
INSERT @table1 (c2, is_transient) VALUES (N'sample durable', 0)
INSERT @table1 (c2, is_transient) VALUES (N'sample non-durable', 1)
EXECUTE dbo.usp_ingest_table1 @table1=@table1
SELECT c1, c2 from dbo.table1
SELECT c1, c2 from dbo.temp_table1
GO

--7. Cleanup
if (@Cleanup)
BEGIN

	DROP PROCEDURE IF EXISTS dbo.usp_ingest_table1;
	DROP TYPE IF EXISTS dbo.tt_table1;
	DROP TABLE IF EXISTS dbo.temp_table1;
	DROP TABLE IF EXISTS dbo.table1;
END