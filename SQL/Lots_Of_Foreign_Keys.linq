<Query Kind="SQL" />

DECLARE 
	@TableName VARCHAR(256), 
	@SchemaName VARCHAR(256),
	@ColumnName VARCHAR(256);

SET @TableName = '';
SET @SchemaName = '';
SET @ColumnName = '';

WITH source AS 
(
	SELECT 
		 t.name [Table],
		s.name [schema],
		@ColumnName [Column]
	FROM sys.tables t
	JOIN sys.schemas s
		ON s.SCHEMA_ID = t.SCHEMA_ID
	WHERE 1=1
	AND t.name = @TableName
	AND s.name = @SchemaName
)

SELECT	'ALTER TABLE ['+ ss.[name] +'].[' + t.[name] + 
		'] WITH CHECK ADD CONSTRAINT [FK_' + t.[name] + '_' + s.[Table]  +
		'] FOREIGN KEY (['+ [Column] +']) ' + 
		'REFERENCES [' + s.[schema] + '].['+ s.[Table] +'] (['+ s.[Column] +'])' 
FROM sys.tables t
CROSS APPLY (SELECT * FROM source) s
JOIN sys.schemas ss
	ON ss.name = s.[schema]
WHERE 1=1
	AND t.name != s.[Table]; --Exclude the Foreign to table
