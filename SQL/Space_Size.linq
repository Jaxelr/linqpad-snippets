<Query Kind="SQL" />

SET NOCOUNT ON;
DBCC UPDATEUSAGE(0)
--DB size.
EXEC sp_spaceused
--Table row counts and sizes.
CREATE TABLE #t 
(
	[name] NVARCHAR(128), 
	[rows] CHAR(11), 
	reserved VARCHAR(18), 
	data VARCHAR(18), 
	index_size VARCHAR(18), 
	unused VARCHAR(18)
) 

INSERT #t EXEC sp_msForEachTable 'EXEC sp_spaceused ''?''' 
SELECT *
FROM #t 
-- # of rows. 
SELECT SUM(CAST([rows] AS int)) AS[rows]
FROM #t 
DROP TABLE #t;
GO
