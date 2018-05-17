<Query Kind="SQL" />

DECLARE @SearchStr VARCHAR(100) 

SELECT @SearchStr = '' ;

SET nocount ON 

SELECT DISTINCT s.name + '.' + Object_name(c.id) AS 'Object name', 
                CASE 
                WHEN Objectproperty(c.id, 'IsReplProc') = 1 
                THEN 'Replication stored procedure' 
                WHEN Objectproperty(c.id, 'IsExtendedProc') = 1 
                THEN 'Extended stored procedure' 
                WHEN Objectproperty(c.id, 'IsProcedure') = 1 
                THEN 'Stored Procedure' 
                WHEN Objectproperty(c.id, 'IsTrigger') = 1 
                THEN 'Trigger' 
                WHEN Objectproperty(c.id, 'IsTableFunction') = 1 
                THEN 'Table-valued function' 
                WHEN Objectproperty(c.id, 'IsScalarFunction') = 1 
                THEN 'Scalar-valued function' 
                WHEN Objectproperty(c.id, 'IsInlineFunction') = 1 
                THEN 'Inline function' 
                END AS 'Object type', 
                'EXEC sp_helptext ''' + s.name + '.' + Object_name(c.id) + '''' 
                'Run this command to see the object text' ,
                [text] 'Resulting text'
FROM   syscomments c 
       JOIN sys.objects o 
         ON c.id = o.object_id 
       JOIN sys.schemas s 
         ON s.schema_id = o.schema_id 
WHERE  c.TEXT LIKE '%' + @SearchStr + '%' 
       AND encrypted = 0 
       AND ( Objectproperty(c.id, 'IsReplProc') = 1 
              OR Objectproperty(c.id, 'IsExtendedProc') = 1 
              OR Objectproperty(c.id, 'IsProcedure') = 1 
              OR Objectproperty(c.id, 'IsTrigger') = 1 
              OR Objectproperty(c.id, 'IsTableFunction') = 1 
              OR Objectproperty(c.id, 'IsScalarFunction') = 1 
              OR Objectproperty(c.id, 'IsInlineFunction') = 1 ) 
ORDER  BY 'Object type', 
          'Object name' ;
GO
