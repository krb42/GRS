/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_Event tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pEvent_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEvent_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEvent_GetALL
AS
BEGIN
    SELECT  e.EventID
        ,   e.EventCode
        ,   e.EventName
        ,   e.[MeetingID]

        ,   e.Deleted
        ,   e.VersionAutoID
        ,   e.TSCreateDate
        ,   e.TSCreateUser
        ,   e.TSModifyDate
        ,   e.TSModifyUser
    FROM dbo.GRS_Event AS e;
END
GO


IF OBJECT_ID('pEvent_GetEventID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEvent_GetEventID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEvent_GetEventID
(
        @EventID int
)
AS
BEGIN
    SELECT  e.EventID
        ,   e.EventCode
        ,   e.EventName
        ,   e.[MeetingID]

        ,   e.Deleted
        ,   e.VersionAutoID
        ,   e.TSCreateDate
        ,   e.TSCreateUser
        ,   e.TSModifyDate
        ,   e.TSModifyUser
    FROM dbo.GRS_Event AS e
    WHERE e.EventID = @EventID;
END
GO


IF OBJECT_ID('pEvent_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEvent_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEvent_Update
(
        @AdminUserID int
    ,   @EventID int
    ,   @EventCode [nvarchar](10)
    ,   @EventName [nvarchar](50)
    ,   @MeetingID [int]
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Event AS e
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_Event
    SET     EventCode = @EventCode
        ,   EventName = @EventName
        ,   MeetingID = @MeetingID

        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE EventID = @EventID;
END
GO


IF OBJECT_ID('pEvent_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEvent_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEvent_Insert
(
        @AdminUserID int
    ,   @EventCode [nvarchar](10)
    ,   @EventName [nvarchar](50)
    ,   @MeetingID [int]
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Event AS e;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_Event
            (EventCode,  EventName,  MeetingID,
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.EventID INTO @generated_keys
    VALUES  (@EventCode, @EventName, @MeetingID, 
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


IF OBJECT_ID('pEvent_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pEvent_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pEvent_Delete
(
        @AdminUserID int
    ,   @EventID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Event AS e;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_Event
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE EventID = @EventID;

END
GO

