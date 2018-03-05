<Query Kind="SQL" />

--First create this func
create function dbo.fk_columns(@constraint_object_id int)
returns varchar(255)
as begin
declare @r varchar(255)
select @r = coalesce(@r + ',', '') + c.name
from sys.foreign_key_columns fkc
join sys.columns c
  on fkc.parent_object_id = c.object_id
  and fkc.parent_column_id = c.column_id
where fkc.constraint_object_id = @constraint_object_id
return @r
end
GO

--Run this after
select distinct
  DropStmt = 'ALTER TABLE [' + ForeignKeys.ForeignTableSchema +
	  '].[' + ForeignKeys.ForeignTableName +
	  '] DROP CONSTRAINT [' + ForeignKeys.ForeignKeyName + '] '
, CreateStmt = 'ALTER TABLE [' + ForeignKeys.ForeignTableSchema +
  '].[' + ForeignKeys.ForeignTableName +
  '] WITH CHECK ADD CONSTRAINT [' + ForeignKeys.ForeignKeyName +
  '] FOREIGN KEY(' + dbo.fk_columns(constraint_object_id) + ')' +
  'REFERENCES [' + schema_name(sys.objects.schema_id) + '].[' +
  sys.objects.[name] + '] '
  + ' ON DELETE CASCADE'
 from sys.objects
  inner join sys.columns
	on (sys.columns.[object_id] = sys.objects.[object_id])
  inner join(
select sys.foreign_keys.[name] as ForeignKeyName
 , schema_name(sys.objects.schema_id) as ForeignTableSchema
 , sys.objects.[name] as ForeignTableName
 , sys.columns.[name]  as ForeignTableColumn
 , sys.foreign_keys.referenced_object_id as referenced_object_id
 , sys.foreign_key_columns.referenced_column_id as referenced_column_id
 , sys.foreign_keys.object_id as constraint_object_id
 from sys.foreign_keys
  inner join sys.foreign_key_columns
	on (sys.foreign_key_columns.constraint_object_id

	  = sys.foreign_keys.[object_id])
  inner join sys.objects
	on(sys.objects.[object_id]

	  = sys.foreign_keys.parent_object_id)

	inner join sys.columns
	  on(sys.columns.[object_id]

		= sys.objects.[object_id])

	   and(sys.columns.column_id
		= sys.foreign_key_columns.parent_column_id)
-- Uncomment this if you want to include only FKs that already
-- have a cascade constraint.
--       where (delete_referential_action_desc = 'CASCADE' or update_referential_action_desc = 'CASCADE')
) ForeignKeys
on(ForeignKeys.referenced_object_id = sys.objects.[object_id])
 and(ForeignKeys.referenced_column_id = sys.columns.column_id)
 where(sys.objects.[type] = 'U')
  and(sys.objects.[name] not in ('sysdiagrams'))
GO
