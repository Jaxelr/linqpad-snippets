<Query Kind="SQL" />

--Replace {searchmehere} with your table name.

SET NOCOUNT ON;
DECLARE @AllTables TABLE(DbName sysname, SchemaName sysname, TableName sysname);
DECLARE
	 @SearchDb NVARCHAR(200)
    ,@SearchSchema NVARCHAR(200)
    ,@SearchTable NVARCHAR(200)
    ,@SQL NVARCHAR(4000);
SET @SearchDb = '%';
SET @SearchSchema = '%';
SET @SearchTable = '%{searchmehere}%';
SET @SQL = 'select ''?'' as DbName, s.name as SchemaName, t.name as TableName from [?].sys.tables t inner join sys.schemas s on t.schema_id=s.schema_id WHERE ''?'' LIKE ''' + @SearchDb + ''' AND s.name LIKE ''' + @SearchSchema + ''' AND t.name LIKE ''' + @SearchTable + '''';

INSERT INTO @AllTables(DbName, SchemaName, TableName) EXECUTE sp_msforeachdb @SQL;

SET NOCOUNT OFF;
SELECT * FROM @AllTables ORDER BY DbName, SchemaName, TableName;