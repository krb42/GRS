/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_Result tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pResult_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pResult_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pResult_GetALL
AS
BEGIN
    SELECT  r.ResultID
        ,   r.MeetingID

        ,   r.SportID
        ,   sp.SportCode
        ,   sp.SportName

        ,   r.EventID
        ,   ev.EventCode
        ,   ev.EventName

        ,   r.DivisionID
        ,   dv.DivisionCode
        ,   dv.DivisionName
        
        ,   r.Participants
        ,   r.DemonstrationOnly

        ,   r.Deleted
        ,   r.VersionAutoID
        ,   r.TSCreateDate
        ,   r.TSCreateUser
        ,   r.TSModifyDate
        ,   r.TSModifyUser
    FROM dbo.GRS_Result AS r
    LEFT JOIN dbo.GRS_Sport AS sp ON sp.SportID = r.SportID
    LEFT JOIN dbo.GRS_Event AS ev ON ev.EventID = r.EventID
    LEFT JOIN dbo.GRS_Division AS dv ON dv.DivisionID = r.DivisionID

END
GO


IF OBJECT_ID('pResult_GetResultID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pResult_GetResultID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pResult_GetResultID
(
        @ResultID int
)
AS
BEGIN
    SELECT  r.ResultID
        ,   r.MeetingID

        ,   r.SportID
        ,   sp.SportCode
        ,   sp.SportName

        ,   r.EventID
        ,   ev.EventCode
        ,   ev.EventName

        ,   r.DivisionID
        ,   dv.DivisionCode
        ,   dv.DivisionName
        
        ,   r.Participants
        ,   r.DemonstrationOnly

        ,   r.Deleted
        ,   r.VersionAutoID
        ,   r.TSCreateDate
        ,   r.TSCreateUser
        ,   r.TSModifyDate
        ,   r.TSModifyUser
    FROM dbo.GRS_Result AS r
    LEFT JOIN dbo.GRS_Sport AS sp ON sp.SportID = r.SportID
    LEFT JOIN dbo.GRS_Event AS ev ON ev.EventID = r.EventID
    LEFT JOIN dbo.GRS_Division AS dv ON dv.DivisionID = r.DivisionID
    WHERE r.ResultID = @ResultID;
END
GO


IF OBJECT_ID('pResult_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pResult_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pResult_Update
(
        @AdminUserID int
    ,   @ResultID int
    ,   @MeetingID int

    ,   @SportID int
    ,   @EventID int
    ,   @DivisionID int
    ,   @Participants int
    ,   @DemonstrationOnly bit
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Result AS e
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_Result
    SET     MeetingID = @MeetingID

        ,   SportID = @SportID
        ,   EventID = @EventID
        ,   DivisionID = @DivisionID
        ,   Participants = @Participants
        ,   DemonstrationOnly = @DemonstrationOnly

        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE ResultID = @ResultID;
END
GO


IF OBJECT_ID('pResult_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pResult_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pResult_Insert
(
        @AdminUserID int
    ,   @MeetingID int

    ,   @SportID int
    ,   @EntityID int
    ,   @DivisionID int
    ,   @Participants int
    ,   @DemonstrationOnly bit
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Result AS e;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_Result
            (MeetingID,  SportID,  EventID,  DivisionID,  Participants,  DemonstrationOnly,
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.EventID INTO @generated_keys
    VALUES  (@MeetingID, @SportID, @EntityID, @DivisionID, @Participants, @DemonstrationOnly,
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


IF OBJECT_ID('pResult_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pResult_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pResult_Delete
(
        @AdminUserID int
    ,   @ResultID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(e.VersionAutoID) FROM dbo.GRS_Result AS e;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_Result
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE ResultID = @ResultID;

END
GO

