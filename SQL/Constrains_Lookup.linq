<Query Kind="SQL" />

--This doesnt work for multiple keys, ive been slacking into refactoring it to a correct structure

SELECT 
--Add Constraints 
'ALTER TABLE [' + s.NAME + '].[' + t.NAME 
+ '] ADD CONSTRAINT [' + f.NAME 
+ '] FOREIGN KEY(' + c.NAME + ') REFERENCES [' 
+ rs.NAME + '].[' + rt.NAME + '] (' + rc.NAME + ');' + CHAR(13)+CHAR(10) + 'GO' AS [Add_Constraints] 
--Remove Constraints 
, 
'IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = ''' + f.Name +''') ALTER TABLE [' + s.NAME + '].[' + t.NAME 
+ '] DROP CONSTRAINT [' + f.NAME + '];' + CHAR(13)+CHAR(10) + 'GO' AS [Remove_Contraints] 
FROM   sys.foreign_keys f 
       INNER JOIN sys.foreign_key_columns AS fk 
               ON f.object_id = fk.constraint_object_id 
       INNER JOIN sys.tables AS t 
               ON fk.parent_object_id = t.object_id 
       INNER JOIN sys.schemas AS s 
               ON s.schema_id = t.schema_id 
       INNER JOIN sys.columns AS c 
               ON fk.parent_object_id = c.object_id 
                  AND fk.parent_column_id = c.column_id 
       INNER JOIN sys.tables AS rt 
               ON rt.object_id = fk.referenced_object_id 
       INNER JOIN sys.schemas AS rs 
               ON rs.schema_id = rt.schema_id 
       INNER JOIN sys.columns AS rc 
               ON rc.object_id = fk.referenced_object_id 
                  AND fk.referenced_column_id = rc.column_id 