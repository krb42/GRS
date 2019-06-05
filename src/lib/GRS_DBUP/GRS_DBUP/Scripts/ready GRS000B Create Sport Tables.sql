/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create GRS tables - GRS_Sport, GRS_Sport_Statistics, GRS_Division, GRS_Event, GRS_Result and GRS_PlaceGetter </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Sport')
BEGIN

    CREATE TABLE [dbo].[GRS_Sport](
        [SportID] [int] IDENTITY(1,1) NOT NULL,
        [SportCode] [nvarchar](10) NOT NULL,
        [SportName] [nvarchar](50) NOT NULL,
        [MeetingID] [int] NOT NULL,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Sport_SportID] PRIMARY KEY CLUSTERED 
            (
                [SportID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Sport_Statistic')
BEGIN

    CREATE TABLE [dbo].[GRS_Sport_Statistic](
        [SportStatisticID] [int] IDENTITY(1,1) NOT NULL,
        [SportID] [int] NOT NULL,

        Stat_LastResult_VersionAutoID int null,

        Stat_Events int null,
        Stat_Nominations int null,
        Stat_Participants int null,

        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Sport_Statistic_SportStatisticID] PRIMARY KEY CLUSTERED 
            (
                [SportStatisticID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Sport_Statistic_GRS_Sport')
BEGIN
    ALTER TABLE [dbo].[GRS_Sport_Statistic] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Sport_Statistic_GRS_Sport] FOREIGN KEY([SportID])
            REFERENCES [dbo].[GRS_Sport] ([SportID])
    ALTER TABLE [dbo].[GRS_Sport_Statistic] CHECK CONSTRAINT [FK_GRS_Sport_Statistic_GRS_Sport]
END
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Division')
BEGIN

    CREATE TABLE [dbo].[GRS_Division](
        [DivisionID] [int] IDENTITY(1,1) NOT NULL,
        [DivisionCode] [nvarchar](10) NOT NULL,
        [DivisionName] [nvarchar](50) NOT NULL,
        [MeetingID] [int] NOT NULL,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Division_DivisionID] PRIMARY KEY CLUSTERED 
            (
                [DivisionID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Event')
BEGIN

    CREATE TABLE [dbo].[GRS_Event](
        [EventID] [int] IDENTITY(1,1) NOT NULL,
        [EventCode] [nvarchar](10) NOT NULL,
        [EventName] [nvarchar](50) NOT NULL,
        [MeetingID] [int] NOT NULL,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Event_EventID] PRIMARY KEY CLUSTERED 
            (
                [EventID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Result')
BEGIN

    CREATE TABLE [dbo].[GRS_Result](
        [ResultID] [int] IDENTITY(1,1) NOT NULL,
        [MeetingID] [int] NOT NULL,
        [SportID] [int] NULL,
        [EventID] [int] NULL,
        [DivisionID] [int] NULL,
        [Participants] int NULL,
        [DemonstrationOnly] bit NULL,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Result_ResultID] PRIMARY KEY CLUSTERED 
            (
                [ResultID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Result_GRS_Sport')
BEGIN
    ALTER TABLE [dbo].[GRS_Result] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Result_GRS_Sport] FOREIGN KEY([SportID])
            REFERENCES [dbo].[GRS_Sport] ([SportID])
    ALTER TABLE [dbo].[GRS_Result] CHECK CONSTRAINT [FK_GRS_Result_GRS_Sport]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Result_GRS_Event')
BEGIN
    ALTER TABLE [dbo].[GRS_Result] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Result_GRS_Event] FOREIGN KEY([EventID])
            REFERENCES [dbo].[GRS_Event] ([EventID])
    ALTER TABLE [dbo].[GRS_Result] CHECK CONSTRAINT [FK_GRS_Result_GRS_Event]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Result_GRS_Division')
BEGIN
    ALTER TABLE [dbo].[GRS_Result] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Result_GRS_Division] FOREIGN KEY([DivisionID])
            REFERENCES [dbo].[GRS_Division] ([DivisionID])
    ALTER TABLE [dbo].[GRS_Result] CHECK CONSTRAINT [FK_GRS_Result_GRS_Division]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_PlaceGetter')
BEGIN

    CREATE TABLE [dbo].[GRS_PlaceGetter](
        [PlaceGetterID] [int] IDENTITY(1,1) NOT NULL,
        [ResultID] [int] NOT NULL,
        [Placing] [int] NULL,
        [TiePlace] [int] NULL,
        [PlaceWeight] float NULL,
        [TeamMemberNumber] [int] NULL,
        [EntityID] [int] NULL,
        [MeetingID] [int] NOT NULL,
        HandicapWeight float null,
        [Name] [nvarchar](200) NULL,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_PlaceGetter_PlaceGetterID] PRIMARY KEY CLUSTERED 
            (
                [PlaceGetterID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_PlaceGetter_GRS_Result')
BEGIN
    ALTER TABLE [dbo].[GRS_PlaceGetter] WITH CHECK
        ADD CONSTRAINT [FK_GRS_PlaceGetter_GRS_Result] FOREIGN KEY([ResultID])
            REFERENCES [dbo].[GRS_Result] ([ResultID])
    ALTER TABLE [dbo].[GRS_PlaceGetter] CHECK CONSTRAINT [FK_GRS_PlaceGetter_GRS_Result]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_PlaceGetter_GRS_Entity')
BEGIN
    ALTER TABLE [dbo].[GRS_PlaceGetter] WITH CHECK
        ADD CONSTRAINT [FK_GRS_PlaceGetter_GRS_Entity] FOREIGN KEY([EntityID])
            REFERENCES [dbo].[GRS_Entity] ([EntityID])
    ALTER TABLE [dbo].[GRS_PlaceGetter] CHECK CONSTRAINT [FK_GRS_PlaceGetter_GRS_Entity]
END
GO



        