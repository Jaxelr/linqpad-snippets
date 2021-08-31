<Query Kind="SQL" />

DROP PROCEDURE IF EXISTS dbo.ExecuteCommands;
GO
DROP TYPE IF EXISTS CommandTableType;
GO

CREATE TYPE CommandTableType AS TABLE(CommandId INT, Command nvarchar(2048))
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO
/* This baby will process all your commands rbar */
CREATE OR ALTER PROC dbo.ExecuteCommands(@Tvp As CommandTableType READONLY)
AS
BEGIN
	SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;
DECLARE @Rows int;
SELECT @Rows = COUNT(*) FROM @Tvp;

WHILE(@Rows > 0)
		BEGIN
			DECLARE @Command nvarchar(2048);

SELECT @Command = Command FROM @Tvp WHERE CommandId = @Rows


			EXEC sp_executesql @Command;

SET @Rows = @Rows - 1
		END
	COMMIT;
END
GO


/* Usage */
DECLARE @Tvp as CommandTableType;

INSERT INTO @Tvp(CommandId, Command)
SELECT ROW_NUMBER() OVER(ORDER BY command), command
FROM
(
   SELECT 'SELECT 1' command union

   SELECT 'SELECT 2' 
) T

EXEC ExecuteCommands @Tvp;