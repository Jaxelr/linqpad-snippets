<Query Kind="SQL" />

declare @table varchar(255), @schema varchar(255), 
		@columns varchar(max), @keycolumns varchar(max),
		@statement nvarchar(max), @filter varchar(max)

SET @table = 'Sampl2'
SET @filter = 'WHERE 1=1 AND Id1 = 1'--This param, dont expose to public interfaces

--Defaults
SET @schema = COALESCE(@schema, 'dbo')
--

--concat all keys
SELECT @keycolumns = COALESCE(concat(@keycolumns, COLUMN_NAME, ','), COLUMN_NAME)
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = @table and TABLE_SCHEMA = @schema

--concat all collumns
SELECT @columns = COALESCE(concat(@columns, COLUMN_NAME, ','), COLUMN_NAME)
FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @table and TABLE_SCHEMA = @schema

--remove the ending commas
SET @keycolumns = LEFT(@keycolumns, LEN(@keycolumns) - 1)
SET @columns = LEFT(@columns, LEN(@columns) - 1)

PRINT 'We gonna run this: ' + concat('SELECT ', @keycolumns, ', HASHBYTES (''SHA'', concat(', @columns, ')) FROM ', @schema, '.', @table, SPACE(1) , @filter)
SET @statement = concat('SELECT ', @keycolumns, ', HASHBYTES (''SHA'', concat(', @columns, ')) FROM ', @schema, '.', @table, SPACE(1), @filter)

exec sp_executesql @statement
