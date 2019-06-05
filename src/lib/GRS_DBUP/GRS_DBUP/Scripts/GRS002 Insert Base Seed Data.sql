/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Insert Base Seed Data for GRS tables - GRS_Meeting and GRS_User  </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF NOT EXISTS (SELECT * FROM GRS_User WHERE UserName = 'Admin')
BEGIN
    SET IDENTITY_INSERT dbo.GRS_User ON
    INSERT INTO GRS_User ([UserID], [UserName], [UserCode], [AccessCode], 
                            [Deleted], [TSCreateDate], [TSCreateUser],       [VersionAutoID])
                    SELECT 1,       'Admin',    'ADMIN',    'snafu',      
                            0,         GETDATE(),      'GRS Basic Seed Data', NEXT VALUE FOR VersionAutoID_Sequence;
    SET IDENTITY_INSERT dbo.GRS_User OFF
END


IF NOT EXISTS (SELECT * FROM GRS_Meeting WHERE MeetingID = 1)
BEGIN
    SET IDENTITY_INSERT dbo.GRS_Meeting ON
    INSERT INTO GRS_Meeting ([MeetingID], [Description],   [Name],               [ReportTitle],                [StartDate], [EndDate], 
                                [Deleted], [TSCreateDate], [TSCreateUser],        [VersionAutoID])
                    SELECT    1,          'Admin Meeting', 'Admin Name Meeting', 'Admin Meeting Report Tiele', GETDATE(),   GETDATE(), 
                                0,         GETDATE(),      'GRS Basic Seed Data', NEXT VALUE FOR VersionAutoID_Sequence;
    SET IDENTITY_INSERT dbo.GRS_Meeting OFF
END





