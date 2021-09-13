<Query Kind="SQL" />

/* Careful with this command since it kills connections! */
DECLARE @dbs TABLE
(
	id INT IDENTITY(1,1) NOT NULL,
    database_id int,
	[name] varchar(256)
)

INSERT INTO	@dbs(name, database_id)
SELECT  name, database_id 
FROM sys.databases
WHERE	database_id > 7

DECLARE @rows int;

SELECT @rows = COUNT(*) FROM @dbs;

WHILE(@rows > 0)
BEGIN
	DECLARE @id int;
	
	SELECT TOP 1 @id  = id FROM @dbs WHERE id = @rows;

	DECLARE @kill varchar(8000) = '';  
	SELECT @kill = CONCAT(@kill , 'kill ' , session_id, ';')
	FROM sys.dm_exec_sessions
	WHERE database_id IN (SELECT database_id FROM @dbs WHERE Id = @id);

	EXEC (@kill)
	--PRINT @kill;

	DECLARE @Backup varchar(256);

	SELECT @Backup = CONCAT('BACKUP DATABASE [' , name ,'] TO DISK = ''', name  , '.bak'' WITH FORMAT')
	from @dbs 
	WHERE Id = @id;

	EXEC (@Backup);
	--PRINT @Backup;
	
	SET @rows = @rows - 1;
END

