<Query Kind="SQL" />

create function [dbo].[MultiSpaceToSingle](@string varchar(255)) returns varchar(255)
as
begin
    declare @o varchar(255) 
	
	--This CAN be buggy if your chars contains >><< which is a weird weird scenario.
    select @o = replace(replace(replace(@string,' ','<>'),'><',''),'<>',' ')

    return @o
end
