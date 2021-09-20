<Query Kind="SQL" />

DROP TABLE IF EXISTS #Temp
GO
CREATE TABLE #Temp
(
    Id INT IDENTITY(1,1) NOT NULL,
	value1 int not null,
	value2 int not null
)

INSERT INTO	#Temp(value1, value2)
SELECT 1, 2 UNION
SELECT 3, 4 UNION
SELECT 6, 5


SELECT  Id, value1, value2,  MAX(field1) MaxField, MIN(field2) MinField
FROM #Temp
CROSS APPLY (VALUES (value1), (value2)) AS Maxy(field1)
CROSS APPLY (VALUES (value1), (value2)) AS Miny(field2)
GROUP BY Id, value1, value2

DROP TABLE IF EXISTS #Temp
GO