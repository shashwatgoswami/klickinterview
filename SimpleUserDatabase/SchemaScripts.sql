create database SimpleUserDatabase;

use SimpleUserDatabase;

create table [dbo].[refProvince] (
[ID] int primary key identity(1,1),
[Name] nvarchar(100) unique not null,
[Code] nvarchar(2) unique not null,
[CountryCode] nvarchar(2) not null
);

create table [dbo].[refMedication] (
[ID] int primary key identity(1,1),
[Name] nvarchar(100)
);

create table [dbo].[refUserState] (
[ID] int primary key identity(1,1),
[State] nvarchar(10)
);

create table [dbo].[User] (
[ID] int primary key identity(1,1),
[Name] nvarchar(200),
[EmailAddress] nvarchar(350),
[ProvinceID] int not null,
[State] int not null,
[DateCreated] datetime2 not null default sysdatetime(),
[LastUpdated] datetime2 not null,
[Deleted] bit default 0,
CONSTRAINT FK_User_Province FOREIGN KEY ([ProvinceID]) REFERENCES [dbo].[refProvince] (ID),
CONSTRAINT FK_User_UserState FOREIGN KEY ([State]) REFERENCES [dbo].[refUserState] (ID)
);

create table [dbo].[UserMedication] (
[ID] int primary key identity(1,1),
[UserID] int not null,
[MedicationID] int not null,
[Deleted] bit  default 0,
[DateCreated] datetime2 not null default sysdatetime(),
[LastUpdated] datetime2 not null,
CONSTRAINT FK_UserMedication_User FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] (ID),
CONSTRAINT FK_UserMedication_Medication FOREIGN KEY ([MedicationID]) REFERENCES [dbo].[refMedication] (ID)
);

CREATE TYPE [dbo].[IDList]
AS TABLE
(
  ID INT
);
GO

create procedure [dbo].[createUser] (
@name nvarchar(200),
@emailAddress nvarchar(350),
@provinceID int,
@userState int,
@medicationList AS [dbo].[IDList] READONLY,
@userID int OUTPUT
)
AS
BEGIN
INSERT INTO [dbo].[User] ([Name], [EmailAddress], [ProvinceID], [State], [LastUpdated])
VALUES (@name, @emailAddress, @provinceID, @userState, SYSDATETIME());

select @userID = SCOPE_IDENTITY();

INSERT INTO [dbo].[UserMedication] ([UserID], [MedicationID], [LastUpdated])
SELECT @userID, ID, SYSDATETIME()
FROM @medicationList
END
GO


create procedure [dbo].[getUser] (
@userID int
)
AS
BEGIN
SELECT USR.[ID],
			USR.[Name],
			USR.[EmailAddress],
			USR.[ProvinceID],
			USR.[State],
			USR.[DateCreated],
			USR.[LastUpdated],
			USR.[Deleted]
		FROM [dbo].[User] USR
		WHERE USR.ID = @userID


SELECT rm.ID, rm.Name
FROM
[dbo].[UserMedication] um
INNER JOIN [dbo].[refMedication] rm ON rm.ID = um.MedicationID
WHERE um.UserID = @userID
AND um.Deleted = 0

END
GO

create procedure [dbo].[getUserMedications] (
@userID int
)
AS
BEGIN

SELECT rm.ID, rm.Name
FROM
[dbo].[UserMedication] um
INNER JOIN [dbo].[refMedication] rm ON rm.ID = um.MedicationID
WHERE um.UserID = @userID
AND um.Deleted = 0

END
GO

create procedure [dbo].[updateUser] (
@userID int,
@name nvarchar(200),
@emailAddress nvarchar(350),
@provinceID int,
@userState int,
@medicationList AS [dbo].[IDList] READONLY
)
AS
BEGIN
UPDATE [dbo].[User] SET [Name] = @name, [EmailAddress] = @emailAddress, [ProvinceID] = @provinceID, [State] = @userState, [LastUpdated] = SYSDATETIME()
where ID = @userID;

INSERT INTO [dbo].[UserMedication] ([UserID], [MedicationID], [LastUpdated])
SELECT @userID, ID, SYSDATETIME()
FROM @medicationList where ID not in (select [MedicationID] from [dbo].[UserMedication] where [UserID] = @userID and Deleted = 0)
END

UPDATE [dbo].[UserMedication] SET Deleted = 1, [LastUpdated] = SYSDATETIME()
WHERE [UserID] = @userID AND [Deleted] = 0 AND [MedicationID] not in (select ID from @medicationList);

GO

create procedure [dbo].[searchUser] (
	@userID int NULL,
	@name nvarchar(200) NULL,
	@emailAddress nvarchar(350) NULL,
	@provinceID int NULL,
	@userState int NULL,
	@medicationList AS [dbo].[IDList] READONLY
)
AS
BEGIN

	DECLARE @medicationCount As int = 0;
	select @medicationCount = count(*) from @medicationList;
	IF @medicationCount > 0
		WITH Med_CTE (UserID)
			AS
			(  
				SELECT UserID
				FROM [dbo].[UserMedication]  
				WHERE MedicationID in (SELECT ID FROM @medicationList) 
				AND Deleted = 0
			)
		SELECT USR.[ID],
			USR.[Name],
			USR.[EmailAddress],
			USR.[ProvinceID],
			USR.[State],
			USR.[DateCreated],
			USR.[LastUpdated],
			USR.[Deleted]
		FROM [dbo].[User] USR
		WHERE USR.ID = IIF(@userID is null, USR.ID, @userID)
		AND USR.[Name] = IIF(@name is null, USR.[Name], @name)
		AND USR.[EmailAddress] = IIF(@emailAddress is null, USR.[EmailAddress], @emailAddress)
		AND USR.[ProvinceID] = IIF(@provinceID is null, USR.[ProvinceID], @provinceID)
		AND USR.[State] = IIF(@userState is null, USR.[State], @userState)
		AND USR.Deleted = 0
		AND USR.[ID] IN (select UserID from Med_CTE group by UserID having count(UserID) = @medicationCount)
	ELSE
		SELECT USR.[ID],
			USR.[Name],
			USR.[EmailAddress],
			USR.[ProvinceID],
			USR.[State],
			USR.[DateCreated],
			USR.[LastUpdated],
			USR.[Deleted]
		FROM [dbo].[User] USR
		WHERE USR.ID = IIF(@userID is null, USR.ID, @userID)
		AND USR.[Name] = IIF(@name is null, USR.[Name], @name)
		AND USR.[EmailAddress] = IIF(@emailAddress is null, USR.[EmailAddress], @emailAddress)
		AND USR.[ProvinceID] = IIF(@provinceID is null, USR.[ProvinceID], @provinceID)
		AND USR.[State] = IIF(@userState is null, USR.[State], @userState)
		AND USR.Deleted = 0
	END
GO
