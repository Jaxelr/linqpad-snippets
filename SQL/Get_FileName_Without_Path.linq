<Query Kind="SQL">
  <Connection>
    <ID>6d34aca6-fbaa-43cb-bafd-f9017d7a1bb0</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>localhost</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>master</Database>
  </Connection>
</Query>

DECLARE @delimiter char(1) = '\';
DECLARE @Input varchar(256) = 'C:\Temp\myFile.txt';
--SET @Input varchar(256) = 'myFile.txt'; --Same result

SET NOCOUNT ON;
SET XACT_ABORT ON;
	
DECLARE @LastIndex int = (LEN(@Input)) - CHARINDEX(@Delimiter, REVERSE(@Input));

--If the record sent doesnt have the delimiter lets se it to -1
SET @LastIndex = IIF(@LastIndex = LEN(@Input), -1, @LastIndex);
DECLARE @FileOnly varchar(256) = RIGHT(@Input, len(@Input) - @LastIndex - 1);

SELECT @FileOnly;