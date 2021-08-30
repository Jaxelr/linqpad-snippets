<Query Kind="SQL" />

IF EXISTS(SELECT* FROM tempdb.sys.objects WHERE [name] LIKE '#WaitResources%') DROP TABLE #WaitResources;
CREATE TABLE #WaitResources (session_id INT, wait_type NVARCHAR(1000), wait_duration_ms INT,
                             resource_description sysname NULL, DB_NAME NVARCHAR(1000), SCHEMA_NAME NVARCHAR(1000),
                             OBJECT_NAME NVARCHAR(1000), index_name NVARCHAR(1000));
GO
DECLARE @WaitDelay VARCHAR(16), @Counter INT, @MaxCount INT, @Counter2 INT;
SELECT @Counter = 0, @MaxCount = 600, @WaitDelay = '00:00:00.100'; /* 600x.1=60 seconds */

SET NOCOUNT ON;
WHILE @Counter<@MaxCount
BEGIN
   INSERT INTO #WaitResources(session_id, wait_type, wait_duration_ms, resource_description), db_name, schema_name, object_name, index_name)
   SELECT   wt.session_id,
			wt.wait_type,
			wt.wait_duration_ms,
			wt.resource_description
      FROM sys.dm_os_waiting_tasks wt
      WHERE wt.wait_type LIKE 'PAGELATCH%' AND wt.session_id <> @@SPID;

SET @Counter = @Counter + 1;
WAITFOR DELAY @WaitDelay;
END;

UPDATE #WaitResources 
      SET DB_NAME = DB_NAME(bd.database_id),
         SCHEMA_NAME = s.name,
         OBJECT_NAME = o.name,
         index_name = i.name

			FROM #WaitResources wt
      JOIN sys.dm_os_buffer_descriptors bd

		 ON bd.database_id = SUBSTRING(wt.resource_description, 0, CHARINDEX(':', wt.resource_description))
            AND bd.FILE_ID = SUBSTRING(wt.resource_description, CHARINDEX(':', wt.resource_description) +1, CHARINDEX(':', wt.resource_description, CHARINDEX(':', wt.resource_description) + 1) - CHARINDEX(':', wt.resource_description) - 1)
            AND bd.page_id = SUBSTRING(wt.resource_description, CHARINDEX(':', wt.resource_description, CHARINDEX(':', wt.resource_description) +1 ) +1, LEN(wt.resource_description) + 1)
            --AND wt.file_index > 0 AND wt.page_index > 0
      JOIN sys.allocation_units au ON bd.allocation_unit_id = AU.allocation_unit_id

	  JOIN sys.partitions p ON au.container_id = p.partition_id

	  JOIN sys.indexes i ON p.index_id = i.index_id AND p.OBJECT_ID = i.OBJECT_ID

	  JOIN sys.objects o ON i.OBJECT_ID = o.OBJECT_ID

	  JOIN sys.schemas s ON o.SCHEMA_ID = s.SCHEMA_ID;

SELECT * FROM #WaitResources ORDER BY wait_duration_ms DESC;
GO

DROP TABLE #WaitResources;
