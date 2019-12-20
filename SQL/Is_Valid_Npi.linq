<Query Kind="SQL" />

DECLARE
	@Npi varchar(10)

SET @Npi = ''

IF(LEN(@Npi) != 10 OR ISNUMERIC(@Npi) = 0)
BEGIN
	SELECT 0;
END

DECLARE @Counter int = 1,
		@Accum int = 0, 
		@Check int = RIGHT(@Npi, 1), 
		@Constant int = 24, 
		@HighNumber int;

WHILE @Counter < LEN(@Npi)
BEGIN
	DECLARE @Digit INT = 0;

	If (@Counter % 2 = 1)
		SET @Digit = SUBSTRING(@npi, @counter, 1) * 2;
	ELSE
		SET @Digit = SUBSTRING(@npi, @counter, 1);
	
	PRINT @Digit

	IF (@Digit >= 10)
		SET @Accum = @Accum + 1 + (@Digit - 10);
	ELSE
		SET @Accum = @Accum + @Digit;
	
	SET @Counter = @Counter + 1;
END

SET @Accum = @Accum + @Constant;
SET @HighNumber = CEILING(@Accum/10.0)*10;			

PRINT @HighNumber
PRINT @Accum

SELECT IIF(@HighNumber - @Accum = @Check, 1, 0);