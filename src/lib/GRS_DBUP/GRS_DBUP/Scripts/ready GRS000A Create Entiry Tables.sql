/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create GRS tables - GRS_Entity and GRS_Entity_Statistics            </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Entity')
BEGIN

    CREATE TABLE [dbo].[GRS_Entity](
        [EntityID] [int] IDENTITY(1,1) NOT NULL,
        [MeetingID] [int] NOT NULL,
        [EntityCode] [nvarchar](10) NOT NULL,
        [EntityName] [nvarchar](50) NOT NULL,

        HandicapWeight float null,
        [Population] int null,
        Distance int null,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,

        CONSTRAINT [PK_GRS_Entity_EntityID] PRIMARY KEY CLUSTERED 
            (
                [EntityID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Entity_Statistics')
BEGIN

    CREATE TABLE [dbo].[GRS_Entity_Statistic](
        [EntityStatisticID] [int] IDENTITY(1,1) NOT NULL,
        [EntityID] [int] NOT NULL,

        Stat_LastResult_VersionAutoID int null,

        Stat_Nominations int null,
        Stat_Gold int null,
        Stat_Silver int null,
        Stat_Bronze int null,
        Stat_HCapGold float null,
        Stat_HCapSilver float null,
        Stat_HCapBronze float null,

        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,

        CONSTRAINT [PK_GRS_Entity_Statistic_EntityStatisticID] PRIMARY KEY CLUSTERED 
            (
                [EntityStatisticID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Entity_Statistics_GRS_Entity')
BEGIN
    ALTER TABLE [dbo].[GRS_Entity_Statistic] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Entity_Statistic_GRS_Entity] FOREIGN KEY([EntityID])
            REFERENCES [dbo].[GRS_Entity] ([EntityID])
    ALTER TABLE [dbo].[GRS_Entity_Statistic] CHECK CONSTRAINT [FK_GRS_Entity_Statistic_GRS_Entity]
END
GO


        