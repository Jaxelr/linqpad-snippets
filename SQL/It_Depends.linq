<Query Kind="SQL" />

IF OBJECT_ID(N'dbo.It_Depends') IS NOT NULL
  DROP FUNCTION dbo.It_Depends
GO

CREATE FUNCTION dbo.IT_Depends (@ObjectName Varchar(200), @ObjectsOnWhichItDepends bit)
RETURNS @References TABLE(
	   ThePath VARCHAR(MAX), --the ancestor objects delimited by a '/'

	   TheFullEntityName VARCHAR(200),
	   TheType VARCHAR(20),
	   iteration INT)
/**
summary:   >
 This Table function returns a a table giving the dependencies of the object whose name
 is supplied as a parameter.
 At the moment, only objects are allowed as a parameter, You can specify whether you
 want those objects that rely on the object, or those on whom the object relies.
compatibility: SQL Server 2005 - SQL Server 2012
 Revisions:
 - Author: Phil Factor
   Version: 1.1
   Modification: Allowed both types of dependencies, returned full detail table
   date: 20 Sep 2015
ToDo: Must add assemblies, must allow entities such as types to be specified
example:
     - code: |
Use AdventureWorks
SELECT  space(iteration * 4) + TheFullEntityName + ' (' + rtrim(TheType) + ')'
FROM    dbo.It_Depends('Employee',0)
ORDER BY ThePath
     - code: |
Select * from dbo.It_Depends('Employee',1)
Select * from dbo.It_Depends('Employee',0)
returns:   >
@references table, which has the name, the type, the display order and th
e 'path' of each dependent object
 
**/
AS
BEGIN
DECLARE @DatabaseDependencies TABLE(
	 EntityName VARCHAR(200),
	 EntityType CHAR(5),
	 DependencyType CHAR(4),
	 TheReferredEntity VARCHAR(200),
	 TheReferredType CHAR(5))


INSERT INTO @DatabaseDependencies(EntityName, EntityType, DependencyType, TheReferredEntity, TheReferredType)
			 -- tables that reference udts

		SELECT object_schema_name(o.object_id) +'.' + o.name, o.type, 'hard', ty.name, 'UDT'

		FROM sys.objects o
				INNER JOIN sys.columns AS c ON c.object_ID = o.object_id

				INNER JOIN sys.types ty ON ty.user_type_id = c.user_type_id

		WHERE is_user_defined = 1

		UNION ALL
              -- udtts that reference udts

		SELECT object_schema_name(tt.type_table_object_id) + '.' + tt.name, 'UDTT', 'hard', ty.name, 'UDT'

		FROM sys.table_types tt

				INNER JOIN sys.columns AS c ON c.object_id = tt.type_table_object_id

				INNER JOIN sys.types ty ON ty.user_type_id = c.user_type_id

		WHERE ty.is_user_defined = 1

		 UNION ALL
              --tables/views that reference triggers

		SELECT object_schema_name(o.object_id) + '.' + o.name, o.type, 'hard', object_schema_name(t.object_id) + '.' + t.name, t.type
		FROM    sys.objects t

				INNER JOIN sys.objects AS o ON o.parent_object_id = t.object_id

		WHERE o.type = 'TR'

		UNION ALL
              -- tables that reference defaults via columns (only default objects)

		SELECT object_schema_name(clmns.object_id) +'.' + object_name(clmns.object_id), 'U', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
	  FROM    sys.objects o

				INNER JOIN sys.columns AS clmns ON clmns.default_object_id = o.object_id

		WHERE o.parent_object_id = 0
        UNION ALL
              -- types that reference defaults (only default objects)

		SELECT types.name, 'UDT', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.objects o

				INNER JOIN sys.types AS types ON types.default_object_id = o.object_id

		WHERE o.parent_object_id = 0

		UNION ALL
              -- tables that reference rules via columns

		SELECT object_schema_name(clmns.object_id) + '.' + object_name(clmns.object_id), 'U', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.objects o

				INNER JOIN sys.columns AS clmns ON clmns.rule_object_id = o.object_id

		UNION ALL          
              -- types that reference rules

		SELECT types.name, 'UDT', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.objects o

				INNER JOIN sys.types AS types ON types.rule_object_id = o.object_id

		UNION ALL
              -- tables that reference XmlSchemaCollections

		SELECT object_schema_name(clmns.object_id) + '.' + object_name(clmns.object_id), 'U', 'hard', xml_schema_collections.name, 'XMLC'

		FROM sys.columns clmns --should we eliminate views?

				INNER JOIN sys.xml_schema_collections ON xml_schema_collections.xml_collection_id = clmns.xml_collection_id

		UNION ALL
              -- table types that reference XmlSchemaCollections
		SELECT  object_schema_name(clmns.object_id) + '.' + object_name(clmns.object_id), 'UDTT', 'hard', xml_schema_collections.name, 'XMLC'

		FROM sys.columns AS clmns
				INNER JOIN sys.table_types AS tt ON tt.type_table_object_id = clmns.object_id

				INNER JOIN sys.xml_schema_collections ON xml_schema_collections.xml_collection_id = clmns.xml_collection_id

		UNION ALL
              -- procedures that reference XmlSchemaCollections

		SELECT object_schema_name(params.object_id) +'.' + o.name, o.type, 'hard', xml_schema_collections.name, 'XMLC'

		FROM sys.parameters AS params
				INNER JOIN sys.xml_schema_collections ON xml_schema_collections.xml_collection_id = params.xml_collection_id
				INNER JOIN sys.objects o ON o.object_id = params.object_id
		UNION ALL
              -- table references table
		SELECT  object_schema_name(tbl.object_id) + '.' + tbl.name, tbl.type, 'hard', object_schema_name(referenced_object_id) + '.' + object_name(referenced_object_id), 'U'

		FROM sys.foreign_keys AS fk
				INNER JOIN sys.tables AS tbl ON tbl.object_id = fk.parent_object_id

		UNION ALL                
 
              -- uda references types
		SELECT  object_schema_name(params.object_id) +'.' + o.name, o.type, 'hard', types.name, 'UDT'

		FROM sys.parameters AS params
				INNER JOIN sys.types ON types.user_type_id = params.user_type_id
				INNER JOIN sys.objects o ON o.object_id = params.object_id
		WHERE   is_user_defined<> 0

		UNION ALL
 
              -- table, view references partition scheme

		SELECT object_schema_name(o.object_id) + '.' + o.name, o.type, 'hard', ps.name, 'PS'

		FROM sys.indexes AS idx
				INNER JOIN sys.partitions p ON idx.object_id = p.object_id AND idx.index_id = p.index_id

				INNER JOIN sys.partition_schemes ps ON idx.data_space_id = ps.data_space_id

				INNER JOIN sys.objects AS o ON o.object_id = idx.object_id

		UNION ALL
 
              -- partition scheme references partition function
		SELECT  ps.name, 'PS', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.partition_schemes ps

				INNER JOIN sys.objects AS o ON ps.function_id = o.object_id

		UNION ALL         
 
              -- plan guide references sp, udf, triggers
		SELECT  pg.name, 'PG', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.objects o

				INNER JOIN sys.plan_guides AS pg ON pg.scope_object_id = o.object_id

		UNION ALL
 
              -- synonym refrences object
		SELECT  s.name, 'SYN', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.objects o

				INNER JOIN sys.synonyms AS s ON object_id(s.base_object_name) = o.object_id

		UNION ALL                       
             
              --  sequences that reference uddts

		SELECT s.name, 'SYN', 'hard', object_schema_name(o.object_id) + '.' + o.name, o.type
		FROM    sys.objects o

				INNER JOIN sys.sequences AS s ON s.user_type_id = o.object_id

		UNION ALL

		SELECT DISTINCT

				coalesce(object_schema_name(Referencing_ID) + '.', '') +object_name(Referencing_ID), referencer.type, 'soft', coalesce(referenced_schema_name + '.', '') + --likely schema name

		 coalesce(referenced_entity_name, ''), --very likely entity name
				referenced.type

		FROM sys.sql_expression_dependencies
				INNER JOIN sys.objects referencer ON referencing_id = referencer.object_ID

				INNER JOIN sys.objects referenced ON referenced_id = referenced.object_ID

		WHERE referencing_Class = 1 AND referenced_class = 1 AND referencer.type IN ( 'v', 'tf', 'fn', 'p', 'tr', 'u' )


DECLARE @RowCount INT
DECLARE @ii INT
--firstly we put in the object as a seed.
INSERT INTO @References(ThePath, TheFullEntityName, theType, iteration)

		SELECT coalesce(object_schema_name(object_ID) + '.', '') +name, coalesce(object_schema_name(object_ID) + '.', '') + name, type, 1

		FROM sys.objects WHERE name LIKE @ObjectName
-- then we just pull out the dependencies at each level.watching out for
-- self-references and circular references
SELECT @rowcount = @@ROWCOUNT, @ii = 2
IF @ObjectsOnWhichItDepends<>0 --if we are looking for objects on which it depends
WHILE @ii< 20 AND @rowcount> 0
  BEGIN
	INSERT  INTO @References (ThePath, TheFullEntityName, theType, iteration)
			SELECT DISTINCT
					ThePath + '/' + TheReferredEntity, TheReferredEntity, TheReferredType, @ii
			FROM    @DatabaseDependencies DatabaseDependencies

					INNER JOIN @References previousReferences

								   ON previousReferences.TheFullEntityName = EntityName

									AND previousReferences.iteration = @ii - 1

					 WHERE TheReferredEntity<>EntityName
					 AND TheReferredEntity NOT IN (SELECT TheFullEntityName FROM @References)

	SELECT @rowcount = @@rowcount
   SELECT  @ii = @ii + 1
  END
ELSE --we are looking for objects that depend on it.
WHILE @ii < 20 AND @rowcount > 0
  BEGIN

	INSERT  INTO @References(ThePath, TheFullEntityName, theType, iteration)

			SELECT DISTINCT

					ThePath + '/' + EntityName, EntityName, TheType, @ii

			FROM    @DatabaseDependencies DatabaseDependencies

					INNER JOIN @References previousReferences

								   ON previousReferences.TheFullEntityName = TheReferredEntity

								   AND previousReferences.iteration = @ii - 1

					 WHERE TheReferredEntity <> EntityName

					 AND EntityName NOT IN(SELECT TheFullEntityName FROM @References)

	SELECT  @rowcount = @@rowcount

	SELECT  @ii = @ii + 1
  END
RETURN
 END
 GO
 