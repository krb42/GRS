/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create CRUD Routines for GRS_PlaceGetter tables             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

--USE [$DatabaseName$]
USE GRSData_DEV
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('pPlaceGetter_GetALL') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pPlaceGetter_GetALL AS SELECT 1')
GO
ALTER PROCEDURE dbo.pPlaceGetter_GetALL
AS
BEGIN
    SELECT  pg.PlaceGetterID
        ,   pg.ResultID
        ,   pg.Placing
        ,   pg.TiePlace
        ,   pg.PlaceWeight
        ,   pg.TeamMemberNumber
        
        ,   pg.EntityID
        ,   e.EntityCode
        ,   e.EntityName
        ,   pg.MeetingID
        ,   pg.HandicapWeight
        ,   pg.[Name]

        ,   pg.Deleted
        ,   pg.VersionAutoID
        ,   pg.TSCreateDate
        ,   pg.TSCreateUser
        ,   pg.TSModifyDate
        ,   pg.TSModifyUser
    FROM dbo.GRS_PlaceGetter AS pg
    LEFT JOIN dbo.GRS_Entity AS e ON e.EntityID = pg.EntityID
END
GO


IF OBJECT_ID('pPlaceGetter_GetPlaceGetterID') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pPlaceGetter_GetPlaceGetterID AS SELECT 1')
GO
ALTER PROCEDURE dbo.pPlaceGetter_GetPlaceGetterID
(
        @PlaceGetterID int
)
AS
BEGIN
    SELECT  pg.PlaceGetterID
        ,   pg.ResultID
        ,   pg.Placing
        ,   pg.TiePlace
        ,   pg.PlaceWeight
        ,   pg.TeamMemberNumber
        
        ,   pg.EntityID
        ,   e.EntityCode
        ,   e.EntityName
        ,   pg.MeetingID
        ,   pg.HandicapWeight
        ,   pg.[Name]

        ,   pg.Deleted
        ,   pg.VersionAutoID
        ,   pg.TSCreateDate
        ,   pg.TSCreateUser
        ,   pg.TSModifyDate
        ,   pg.TSModifyUser
    FROM dbo.GRS_PlaceGetter AS pg
    LEFT JOIN dbo.GRS_Entity AS e ON e.EntityID = pg.EntityID
    WHERE pg.PlaceGetterID = @PlaceGetterID;
END
GO


IF OBJECT_ID('pPlaceGetter_Update') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pPlaceGetter_Update AS SELECT 1')
GO
ALTER PROCEDURE dbo.pPlaceGetter_Update
(
        @AdminUserID int
    ,   @PlaceGetterID int
    ,   @ResultID int
    ,   @Placing int
    ,   @TiePlace int
    ,   @PlaceWeight float
    ,   @TeamMemberNumber int
    ,   @EntityID int

    ,   @EntityCode nvarchar(10)
    ,   @EntityName nvarchar(50)
    ,   @MeetingID int
    ,   @HandicapWeight float
    ,   @Name nvarchar(200)
)
AS
BEGIN
    DECLARE @VersionAutoID int
    SELECT @VersionAutoID = MAX(pg.VersionAutoID) FROM dbo.GRS_PlaceGetter AS pg
    SET @VersionAutoID += 1

    DECLARE @AdminUserName nvarchar(80) = NULL
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID

    UPDATE dbo.GRS_PlaceGetter
    SET     ResultID = @ResultID
        ,   Placing = @Placing
        ,   TiePlace = @TiePlace
        ,   PlaceWeight = @PlaceWeight
        ,   TeamMemberNumber = @TeamMemberNumber
        
        ,   EntityID = @EntityID
        ,   MeetingID = @MeetingID
        ,   HandicapWeight = @HandicapWeight
        ,   [Name] = @Name

        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE PlaceGetterID = @PlaceGetterID;
END
GO


IF OBJECT_ID('pPlaceGetter_Insert') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pPlaceGetter_Insert AS SELECT 1')
GO
ALTER PROCEDURE dbo.pPlaceGetter_Insert
(
        @AdminUserID int
    ,   @ResultID int
    ,   @Placing int
    ,   @TiePlace int
    ,   @PlaceWeight float
    ,   @TeamMemberNumber int
    ,   @EntityID int

    ,   @MeetingID int
    ,   @HandicapWeight float
    ,   @Name nvarchar(200)
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(pg.VersionAutoID) FROM dbo.GRS_PlaceGetter AS pg
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    DECLARE @generated_keys table([Id] int);

    INSERT INTO dbo.GRS_PlaceGetter
            (Placing,  TiePlace,  PlaceWeight,  TeamMemberNumber,
             EntityID,  MeetingID,  HandicapWeight, [Name],
             Deleted, VersionAutoID,  TSCreateDate,  TSCreateUser
            )
    OUTPUT inserted.PlaceGetterID INTO @generated_keys
    VALUES  (@Placing, @TiePlace, @PlaceWeight, @TeamMemberNumber,
             @EntityID, @MeetingID, @HandicapWeight, @Name,
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


IF OBJECT_ID('pPlaceGetter_Delete') IS NULL
    EXEC ('CREATE PROCEDURE DBO.pPlaceGetter_Delete AS SELECT 1')
GO
ALTER PROCEDURE dbo.pPlaceGetter_Delete
(
        @AdminUserID int
    ,   @PlaceGetterID int
)
AS
BEGIN
    DECLARE @VersionAutoID int;
    SELECT @VersionAutoID = MAX(pg.VersionAutoID) FROM dbo.GRS_PlaceGetter AS pg
    SET @VersionAutoID += 1;

    DECLARE @AdminUserName nvarchar(80) = NULL;
    SELECT @AdminUserName = u.UserName FROM dbo.GRS_User AS u WHERE u.UserID = @AdminUserID;

    UPDATE dbo.GRS_PlaceGetter
    SET     Deleted = 1
        ,   VersionAutoID = @VersionAutoID
        ,   TSModifyDate = GETDATE()
        ,   TSModifyUser = @AdminUserName
    WHERE PlaceGetterID = @PlaceGetterID;

END
GO

