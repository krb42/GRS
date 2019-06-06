/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_Sport tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pSport_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pSport_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pSport_GetALL
AS
BEGIN
    SELECT  sp.SportID
        ,   sp.SportCode
        ,   sp.SportName
        ,   sp.[MeetingID]

        ,   sp.Stat_Nominations
        ,   sp.Stat_Participants

        ,   sp.Deleted
        ,   sp.VersionAutoID
        ,   sp.TSCreateDate
        ,   sp.TSCreateUser
        ,   sp.TSModifyDate
        ,   sp.TSModifyUser
    FROM dbo.GRS_Sport AS sp
END
GO


IF OBJECT_ID('pSport_GetSportID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pSport_GetSportID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pSport_GetSportID
(
        @SportID int
)
AS
BEGIN
    SELECT  sp.SportID
        ,   sp.SportCode
        ,   sp.SportName
        ,   sp.[MeetingID]

        ,   sp.Stat_Nominations
        ,   sp.Stat_Participants

        ,   sp.Deleted
        ,   sp.VersionAutoID
        ,   sp.TSCreateDate
        ,   sp.TSCreateUser
        ,   sp.TSModifyDate
        ,   sp.TSModifyUser
    FROM dbo.GRS_Sport AS sp
    WHERE sp.SportID = @SportID;
END
GO


IF OBJECT_ID('pSport_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pSport_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pSport_Update
(
        @AdminUserID int
    ,   @SportID int
    ,   @SportCode [nvarchar](10)
    ,   @SportName [nvarchar](50)
    ,   @MeetingID [int]

    ,   @Deleted bit
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(sp.VersionAutoID) FROM dbo.GRS_Sport AS sp
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_Sport
    SET     SportCode = @SportCode
        ,   SportName = @SportName
        ,   MeetingID = @MeetingID

        ,   Deleted = @Deleted
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE SportID = @SportID;
END
GO


IF OBJECT_ID('pSport_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pSport_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pSport_Insert
(
        @AdminUserID int
    ,   @SportCode [nvarchar](10)
    ,   @SportName [nvarchar](50)
    ,   @MeetingID [int]

    ,   @Deleted bit
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(sp.VersionAutoID) FROM dbo.GRS_Sport AS sp;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_Sport
            (SportCode,  SportName,  MeetingID,
             Stat_Nominations, Stat_Participants, 
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.SportID INTO @generated_keys
    VALUES  (@SportCode, @SportName, @MeetingID, 
             0.0,              0.0,
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


IF OBJECT_ID('pSport_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pSport_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pSport_Delete
(
        @AdminUserID int
    ,   @SportID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(sp.VersionAutoID) FROM dbo.GRS_Sport AS sp;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_Sport
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE SportID = @SportID;

END
GO

