<Query Kind="Expression" />

USE[MC21]
GO
/****** Object:  UserDefinedFunction [dbo].[RemoveLatinChars]    Script Date: 8/21/2019 9:53:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION[dbo].[RemoveLatinChars]
(
	@inputString varchar(255)
)
RETURNS varchar(255)
AS
BEGIN
	RETURN
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		REPLACE(
		@inputString COLLATE Latin1_General_CS_AS, //This ensures the collation is case sensitive on comparison for latin chars
		'Ñ', 'N'),
		'É', 'E'),
		'Ó', 'O'),
		'Í', 'I'),
		'Ú', 'U'),
		'Á', 'A'),
		'ñ', 'n'),
		'é', 'e'),
		'ó', 'o'),
		'í', 'i'),
		'ú', 'u'),
		'á', 'a')
END
