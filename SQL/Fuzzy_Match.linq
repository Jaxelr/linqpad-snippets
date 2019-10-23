<Query Kind="SQL" />

IF OBJECT_ID('FuzzyMatch') > 0  DROP FUNCTION[dbo].FuzzyMatch;
GO

CREATE FUNCTION[dbo].FuzzyMatch(@base VARCHAR(512), @match VARCHAR(512))
RETURNS INT
AS
BEGIN

	DECLARE @counter INT, @accumulator INT;

SET @counter = 1;
SET @accumulator = 0;

WHILE @counter <= LEN(@base)

	BEGIN
		IF CHARINDEX(SUBSTRING(@base, @counter, 2), @match) > 0
		BEGIN
			SET @accumulator = @accumulator + 1;
END

SET @counter = @counter + 1;
END;

RETURN @accumulator;
END;
GO