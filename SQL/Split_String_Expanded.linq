<Query Kind="SQL" />

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--This function expands the splitstring function to include the numeric position of the list
CREATE FUNCTION [dbo].[SplitString]
(
   @List NVARCHAR(MAX),
   @Delimiter NVARCHAR(255)
)
RETURNS TABLE
WITH SCHEMABINDING AS
RETURN
	SELECT  
		rn = ROW_NUMBER() OVER (ORDER BY (SELECT NULL)), 
		item = VALUE 
	FROM string_split(@List, @Delimiter);
GO