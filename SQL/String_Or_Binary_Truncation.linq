<Query Kind="SQL" />

--Taken from this Brent Ozar Article: https://www.brentozar.com/archive/2019/03/how-to-fix-the-error-string-or-binary-data-would-be-truncated/

--SQL Server 2019+
ALTER DATABASE SCOPED CONFIGURATION SET VERBOSE_TRUNCATION_WARNINGS = ON;

--SQL Server 2016+
DBCC TRACEON(460, -1);
GO

--Turn OFF once used since this can cause weird bugs

DBCC TRACEOFF(460, -1);
GO