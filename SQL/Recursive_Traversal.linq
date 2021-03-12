<Query Kind="SQL">
  <Connection>
    <ID>a7e80774-6de5-4a3c-9f1d-f0e4d90a197d</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>master</Database>
  </Connection>
</Query>

DECLARE @from date = '2015-01-01'
DECLARE @to date = '2015-01-31'

	;WITH dates(n, field) AS 
	(
		SELECT 0, @from
		UNION ALL
		SELECT (n+1), DATEADD(DAY, n+1, @from)
		FROM dates
		WHERE 1=1
			AND DATEADD(DAY, n+1, @from) <= @to
	)

	SELECT n, field FROM dates OPTION (maxrecursion 0);
