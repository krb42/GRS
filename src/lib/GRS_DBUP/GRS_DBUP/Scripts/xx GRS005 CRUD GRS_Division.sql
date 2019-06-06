/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_Division tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pDivision_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pDivision_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pDivision_GetALL
AS
BEGIN
    SELECT  d.DivisionID
        ,   d.DivisionCode
        ,   d.DivisionName
        ,   d.[MeetingID]

        ,   d.Deleted
        ,   d.VersionAutoID
        ,   d.TSCreateDate
        ,   d.TSCreateUser
        ,   d.TSModifyDate
        ,   d.TSModifyUser
    FROM dbo.GRS_Division AS d;
END
GO


IF OBJECT_ID('pDivision_GetDivisionID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pDivision_GetDivisionID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pDivision_GetDivisionID
(
        @DivisionID int
)
AS
BEGIN
    SELECT  d.DivisionID
        ,   d.DivisionCode
        ,   d.DivisionName
        ,   d.[MeetingID]

        ,   d.Deleted
        ,   d.VersionAutoID
        ,   d.TSCreateDate
        ,   d.TSCreateUser
        ,   d.TSModifyDate
        ,   d.TSModifyUser
    FROM dbo.GRS_Division AS d
    WHERE d.DivisionID = @DivisionID;
END
GO


IF OBJECT_ID('pDivision_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pDivision_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pDivision_Update
(
        @AdminUserID int
    ,   @DivisionID int
    ,   @DivisionCode [nvarchar](10)
    ,   @DivisionName [nvarchar](50)
    ,   @MeetingID [int]
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(d.VersionAutoID) FROM dbo.GRS_Division AS d
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_Division
    SET     DivisionCode = @DivisionCode
        ,   DivisionName = @DivisionName
        ,   MeetingID = @MeetingID

        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE DivisionID = @DivisionID;
END
GO


IF OBJECT_ID('pDivision_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pDivision_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pDivision_Insert
(
        @AdminUserID int
    ,   @DivisionCode [nvarchar](10)
    ,   @DivisionName [nvarchar](50)
    ,   @MeetingID [int]
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(d.VersionAutoID) FROM dbo.GRS_Division AS d;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_Division
            (DivisionCode,  DivisionName,  MeetingID,
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.DivisionID INTO @generated_keys
    VALUES  (@DivisionCode, @DivisionName, @MeetingID, 
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


IF OBJECT_ID('pDivision_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pDivision_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pDivision_Delete
(
        @AdminUserID int
    ,   @DivisionID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(d.VersionAutoID) FROM dbo.GRS_Division AS d;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_Division
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE DivisionID = @DivisionID;

END
GO

