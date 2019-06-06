/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create GRS tables - GRS_Meeting and GRS_User             </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Meeting')
BEGIN
    CREATE TABLE [dbo].[GRS_Meeting](
        [MeetingID] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nchar](80) NOT NULL,
        [Description] [nvarchar](80) NOT NULL,
        [ReportTitle] [nvarchar](80) NOT NULL,

        [StartDate] [date] NOT NULL,
        [EndDate] [date] NOT NULL,

        [Deleted] bit null,
        [VersionAutoID] bigint null,

        [TSCreateDate] datetime null,
        [TSCreateUser] [nvarchar](80) NOT NULL,
        [TSModifyDate] datetime null,
        [TSModifyUser] [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Meeting_MeetingID] PRIMARY KEY CLUSTERED 
            (
                [MeetingID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO


IF NOT EXISTS(SELECT * FROM SYS.INDEXES WHERE [NAME] = 'IX_GRS_Meeting_Name' AND [OBJECT_ID] = OBJECT_ID('GRS_Meeting'))
BEGIN
   CREATE UNIQUE NONCLUSTERED INDEX [IX_GRS_Meeting_Name] ON [GRS_Meeting]
   (
      [Name] ASC
   ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, 
                  IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_User')
BEGIN
    CREATE TABLE [dbo].[GRS_User](
        [UserID] [int] IDENTITY(1,1) NOT NULL,
        [UserName] [nvarchar](80) NOT NULL,
        [UserCode] [nvarchar](80) NOT NULL,
        [AccessCode] [nvarchar](80) NOT NULL,

        [Deleted] bit null,
        [VersionAutoID] bigint null,

        [TSCreateDate] datetime null,
        [TSCreateUser] [nvarchar](80) NOT NULL,
        [TSModifyDate] datetime null,
        [TSModifyUser] [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_User_UserID] PRIMARY KEY CLUSTERED 
            (
                [UserID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO


