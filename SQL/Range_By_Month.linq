<Query Kind="SQL" />

--From Range A to Range B calculate each FoMonth
CREATE OR ALTER FUNCTION dbo.RangeByMonth
(
	@FromDate date,
	@ToDate date
)

RETURNS @Ranges TABLE
	(
		id INT IDENTITY(1,1) NOT NULL,
		value date NOT NULL
	)
AS 
BEGIN
	WHILE(DATEDIFF(Month, @FromDate, @ToDate) >= 0)
	BEGIN
		INSERT INTO	@Ranges
		SELECT DATEADD(month, DATEDIFF(month, 0, @ToDate), 0)

		SET @ToDate = DATEADD(Month, -1, @ToDate);
	END

	RETURN;
END
