<Query Kind="SQL" />

DECLARE @Commands TABLE
(
	CommandId INT IDENTITY(1, 1) NOT NULL,
	Command nvarchar(2048) NOT NULL
)

INSERT INTO @Commands
SELECT  '--Command to execute goes here'

DECLARE @Rows int;

SELECT @Rows = COUNT(*) FROM @Commands;

WHILE(@Rows > 0)
BEGIN
	DECLARE @Command nvarchar(2048);

SELECT @Command = Command FROM @Commands WHERE CommandId = @Rows


	EXEC sp_executesql @Command;

DELETE FROM @Commands WHERE @Rows = CommandId

	SET @Rows = @Rows - 1
END
