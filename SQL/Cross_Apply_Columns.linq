<Query Kind="SQL" />

--Instead of using Unpivot 

DECLARE @workdays TABLE
(
    [Name] varchar(128),
	[FromMonday] time,
	[ToMonday] time,
	[FromTuesday] time,
	[ToTuesday] time,
	[FromWednesday] time,
	[ToWednesday] time,
	[FromThursday] time,
	[ToThursday] time,
	[FromFriday] time,
	[ToFriday] time,
	[FromSaturday] time,
	[ToSaturday] time
)

INSERT INTO	@workdays
SELECT 'Jaxel', 
	'08:00', '17:00', 
	'09:00', '18:00', 
	'08:00', '17:00', 
	'09:00', '18:00', 
	'08:00', '17:00',
	'00:00', '00:00' 

SELECT [Name], [Day], [From], [To]
FROM @workdays
CROSS APPLY 
(
	VALUES ('Monday', [FromMonday], [ToMonday]),
		   ('Tuesday', [FromTuesday], [ToTuesday]),
		   ('Wednesday', [FromWednesday], [ToWednesday]),
	       ('Thursday', [FromThursday], [ToThursday]),
	       ('Friday', [FromFriday], [ToFriday]),
	       ('Saturday', [FromSaturday], [ToSaturday])
) ca ([Day], [From], [To])