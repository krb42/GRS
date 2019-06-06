/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_User tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pUser_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pUser_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pUser_GetALL
AS
BEGIN
    SELECT  u.UserID
        ,   u.UserCode
        ,   u.UserName

        ,   u.Deleted
        ,   u.VersionAutoID
        ,   u.TSCreateDate
        ,   u.TSCreateUser
        ,   u.TSModifyDate
        ,   u.TSModifyUser
    FROM dbo.GRS_User AS u;
END
GO


IF OBJECT_ID('pUser_GetUserID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pUser_GetUserID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pUser_GetUserID
(
        @UserID int
)
AS
BEGIN
    SELECT  u.UserID
        ,   u.UserCode
        ,   u.UserName

        ,   u.Deleted
        ,   u.VersionAutoID
        ,   u.TSCreateDate
        ,   u.TSCreateUser
        ,   u.TSModifyDate
        ,   u.TSModifyUser
    FROM dbo.GRS_User AS u
    WHERE u.UserID = @UserID;
END
GO


IF OBJECT_ID('pUser_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pUser_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pUser_Update
(
        @AdminUserID int
    ,   @UserID int
    ,   @UserCode [nvarchar](10)
    ,   @UserName [nvarchar](50)
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(u.VersionAutoID) FROM dbo.GRS_User AS u
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_User
    SET     UserCode = @UserCode
        ,   UserName = @UserName

        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE UserID = @UserID;
END
GO


IF OBJECT_ID('pUser_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pUser_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pUser_Insert
(
        @AdminUserID int
    ,   @UserCode [nvarchar](10)
    ,   @UserName [nvarchar](50)
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(u.VersionAutoID) FROM dbo.GRS_User AS u
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_User
            (UserCode,  UserName,
             Deleted, VersionAutoID,  TSCreateDate, TSModifyUser
            )
    OUTPUT inserted.UserID INTO @generated_keys
    VALUES  (@UserCode, @UserName, 
             0,       @VersionAutoID, GETDATE(),    @AdminUserName
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


IF OBJECT_ID('pUser_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pUser_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pUser_Delete
(
        @AdminUserID int
    ,   @UserID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(u.VersionAutoID) FROM dbo.GRS_User AS u
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_User
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE UserID = @UserID;

END
GO

