/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_Attribute tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pAttribute_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pAttribute_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pAttribute_GetALL
AS
BEGIN
    SELECT  att.AttributeID
        ,   att.AttributeType
        ,   att.AttributeName
        ,   att.AttributeSetting
        ,   att.ParentID

        ,   att.Deleted
        ,   att.VersionAutoID
        ,   att.TSCreateDate
        ,   att.TSCreateUser
        ,   att.TSModifyDate
        ,   att.TSModifyUser
    FROM dbo.GRS_Attribute AS att;
END
GO


IF OBJECT_ID('pAttribute_GetAttributeID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pAttribute_GetAttributeID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pAttribute_GetAttributeID
(
        @AttributeID int
)
AS
BEGIN
    SELECT  att.AttributeID
        ,   att.AttributeType
        ,   att.AttributeName
        ,   att.AttributeSetting
        ,   att.ParentID

        ,   att.Deleted
        ,   att.VersionAutoID
        ,   att.TSCreateDate
        ,   att.TSCreateUser
        ,   att.TSModifyDate
        ,   att.TSModifyUser
    FROM dbo.GRS_Attribute AS att
    WHERE att.AttributeID = @AttributeID;
END
GO


IF OBJECT_ID('pAttribute_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pAttribute_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pAttribute_Update
(
        @AdminUserID int
    ,   @AttributeID int
    ,   @AttributeType [nvarchar](10)
    ,   @AttributeName [nvarchar](50)
    ,   @AttributeSetting [nvarchar](1000)
    ,   @ParentID [int]
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(att.VersionAutoID) FROM dbo.GRS_Attribute AS att
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_Attribute
    SET     AttributeType = @AttributeType
        ,   AttributeName = @AttributeName
        ,   AttributeSetting = @AttributeSetting
        ,   ParentID = @ParentID

        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE AttributeID = @AttributeID;
END
GO


IF OBJECT_ID('pAttribute_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pAttribute_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pAttribute_Insert
(
        @AdminUserID int
    ,   @AttributeType [nvarchar](10)
    ,   @AttributeName [nvarchar](50)
    ,   @AttributeSetting [nvarchar](1000)
    ,   @ParentID [int]
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(att.VersionAutoID) FROM dbo.GRS_Attribute AS att;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_Attribute
            (AttributeType,  AttributeName,  AttributeSetting,  ParentID,
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.AttributeID INTO @generated_keys
    VALUES  (@AttributeType, @AttributeName, @AttributeSetting, @ParentID, 
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


IF OBJECT_ID('pAttribute_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pAttribute_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pAttribute_Delete
(
        @AdminUserID int
    ,   @AttributeID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(att.VersionAutoID) FROM dbo.GRS_Attribute AS att;
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_Attribute
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE AttributeID = @AttributeID;

END
GO

