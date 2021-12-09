<Query Kind="SQL" />

--Replace {searchmehere} with your table name.

SET NOCOUNT ON;
DECLARE @AllTables TABLE(DbName sysname, SchemaName sysname, TableName sysname);
DECLARE
	 @SearchDb NVARCHAR(200)
    ,@SearchSchema NVARCHAR(200)
    ,@SearchTable NVARCHAR(200)
    ,@SQL NVARCHAR(4000);
	
/* Leave empty if optional */
SET @SearchTable = N'{searchmehere}';
SET @SearchSchema = N''; 
SET @SearchDb = N'';

SET @SQL = CONCAT(N'SELECT  ''?'' as DbName, s.name as SchemaName, t.name as TableName from [?].sys.tables t
			inner join sys.schemas s on t.schema_id=s.schema_id 	
			where 1=1 
				and t.name like ''%'' + ''' , @SearchTable , N''' + ''%'' and s.name like ''%'' + ''', 
				@SearchSchema, N''' + ''%'' and ''?'' like ''%'' + ''', @SearchDb,  N''' + ''%''');

INSERT INTO @AllTables(DbName, SchemaName, TableName) EXECUTE sp_msforeachdb @SQL;

SELECT * FROM @AllTables ORDER BY DbName, SchemaName, TableName;

SET NOCOUNT OFF;