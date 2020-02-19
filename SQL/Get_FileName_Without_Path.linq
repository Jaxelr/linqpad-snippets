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

SELECT right(@Input, charindex(@delimiter, reverse(@Input) + @delimiter) - 1);