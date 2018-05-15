<Query Kind="SQL" />

DECLARE @Sort TABLE
(
    id INT NOT NULL,
	descr nvarchar(25)  NOT NULL
)

INSERT INTO	@Sort
SELECT 1, 'One' UNION
SELECT 2, 'Two' UNION
SELECT 3, 'Three'

DECLARE @data TABLE
(
    datid int,
	descr nvarchar(12),
	sort nvarchar(25)
)

INSERT INTO	@data(datid, descr, sort)
SELECT 1, 'Data 1', 'One' UNION
SELECT 1, 'Data 1', 'Two' UNION
SELECT 2, 'Data 2', 'One' UNION
SELECT 2, 'Data 2', 'Two' UNION
SELECT 2, 'Data 2', 'Three' UNION
SELECT 3, 'Data 3', 'Two';

with lowest as 
(
	SELECT RANK() OVER (PARTITION BY datid ORDER BY s.id) rnk, datid, sort
	FROM @data c
	INNER JOIN @sort s ON c.sort = s.descr
)

SELECT  c.*
FROM @data c
INNER JOIN lowest l
	ON c.datid = l.datid
	AND c.sort = l.sort
WHERE rnk = 1