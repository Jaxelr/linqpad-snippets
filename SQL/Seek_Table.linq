<Query Kind="SQL" />

DECLARE @name varchar(256)

SELECT @name = '';

SELECT c.name AS ColName,
       CASE
              WHEN c.is_nullable = 1
              THEN 'NULL'
              ELSE 'NOT NULL'
       END    AS Nullable,
       s.name + '.' + t.name AS TableName
FROM   sys.columns c
       JOIN sys.tables t
       ON     c.object_id = t.object_id
       JOIN sys.schemas s
       ON	  s.schema_id = t.schema_id       
WHERE  c.name          LIKE '%' + @name +'%';
GO
