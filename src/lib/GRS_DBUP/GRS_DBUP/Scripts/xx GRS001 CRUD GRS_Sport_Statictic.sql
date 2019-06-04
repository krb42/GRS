/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_Entity tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pEntity_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEntity_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEntity_GetALL
AS
BEGIN
    SELECT  e.EntityID
        ,   e.EntityCode
        ,   e.EntityName
        ,   e.[MeetingID]

        ,   e.HandicapWeight
        ,   e.[Population]
        ,   e.Distance

        ,   e.Stat_Nominations
        ,   e.Stat_Gold
        ,   e.Stat_Silver
        ,   e.Stat_Bronze
        ,   e.Stat_HCapGold
        ,   e.Stat_HCapSilver
        ,   e.Stat_HCapBronze

        ,   e.Deleted
        ,   e.VersionAutoID
        ,   e.TSCreateDate
        ,   e.TSCreateUser
        ,   e.TSModifyDate
        ,   e.TSModifyUser
    FROM dbo.GRS_Entity AS e;
END
GO


IF OBJECT_ID('pEntity_GetEntityID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEntity_GetEntityID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEntity_GetEntityID
(
        @EntityID int
)
AS
BEGIN
    SELECT  e.EntityID
        ,   e.EntityCode
        ,   e.EntityName
        ,   e.[MeetingID]

        ,   e.HandicapWeight
        ,   e.[Population]
        ,   e.Distance

        ,   e.Stat_Nominations
        ,   e.Stat_Gold
        ,   e.Stat_Silver
        ,   e.Stat_Bronze
        ,   e.Stat_HCapGold
        ,   e.Stat_HCapSilver
        ,   e.Stat_HCapBronze

        ,   e.Deleted
        ,   e.VersionAutoID
        ,   e.TSCreateDate
        ,   e.TSCreateUser
        ,   e.TSModifyDate
        ,   e.TSModifyUser
    FROM dbo.GRS_Entity AS e
    WHERE e.EntityID = @EntityID;
END
GO


IF OBJECT_ID('pEntity_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEntity_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEntity_Update
(
        @AdminUserID int
    ,   @EntityID int
    ,   @EntityCode [nvarchar](10)
    ,   @EntityName [nvarchar](50)
    ,   @MeetingID [int]

    ,   @HandicapWeight float
    ,   @Population int
    ,   @Distance int

    ,   @Deleted bit
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Entity AS e
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_Entity
    SET     EntityCode = @EntityCode
        ,   EntityName = @EntityName
        ,   MeetingID = @MeetingID

        ,   HandicapWeight = @HandicapWeight
        ,   [Population] = @Population
        ,   Distance = @Distance

        ,   Deleted = @Deleted
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE EntityID = @EntityID;
END
GO


IF OBJECT_ID('pEntity_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEntity_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEntity_Insert
(
        @AdminUserID int
    ,   @EntityCode [nvarchar](10)
    ,   @EntityName [nvarchar](50)
    ,   @MeetingID [int]

    ,   @HandicapWeight float
    ,   @Population int
    ,   @Distance int

    ,   @Deleted bit
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Entity AS e;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_Entity
            (EntityCode,  EntityName,  MeetingID,  HandicapWeight,  [Population], Distance,
             Stat_Nominations, Stat_Gold, Stat_Silver, Stat_Bronze, Stat_HCapGold, Stat_HCapSilver, Stat_HCapBronze,
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.EntityID INTO @generated_keys
    VALUES  (@EntityCode, @EntityName, @MeetingID, @HandicapWeight, @Population,  @Distance,
             0.0,              0.0,       0.0,          0.0,        0.0,           0.0,             0.0,
             0,       @VersionAutoID, GETDATE(),     @AdminUserName
            );

    IF EXISTS(SELECT * FROM @generated_keys)
    BEGIN
        SELECT Success = 1, Id FROM @generated_keys
    END
    ELSE
    BEGIN
        SELECT Success = 0, Id = -1
    END

END
GO


IF OBJECT_ID('pEntity_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEntity_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEntity_Delete
(
        @AdminUserID int
    ,   @EntityID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Entity AS e;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_Entity
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE EntityID = @EntityID;

END
GO

