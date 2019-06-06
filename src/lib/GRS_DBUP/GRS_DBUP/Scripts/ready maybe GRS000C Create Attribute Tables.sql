/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create GRS tables - GRS_Attribute, GRS_Entity_Attribute and GRS_Sport_Attribute        </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Attribute')
BEGIN

    CREATE TABLE [dbo].[GRS_Attribute](
        [AttributeID] [int] IDENTITY(1,1) NOT NULL,
        [AttributeType] [nvarchar](50) NOT NULL,
        [AttributeName] [nvarchar](50) NOT NULL,
        [AttributeSetting] [nvarchar](1000) NULL,

        ParentID int null,

        Deleted bit null,
        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Attribute_AttributeID] PRIMARY KEY CLUSTERED 
            (
                [AttributeID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO		


IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Entity_Attribute')
BEGIN

    CREATE TABLE [dbo].[GRS_Entity_Attribute](
        [EntityAttributeID] [int] IDENTITY(1,1) NOT NULL,
        [EntityID] int NOT NULL,
        [AttributeID] int NOT NULL,
        [AttributeName] [nvarchar](50) NOT NULL,
        [AttributeSetting] [nvarchar](1000) NULL,

        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Entity_Attribute_EntityAttributeID] PRIMARY KEY CLUSTERED 
            (
                [EntityAttributeID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Entity_Attribute_GRS_Attribute')
BEGIN
    ALTER TABLE [dbo].[GRS_Entity_Attribute] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Entity_Attribute_GRS_Attribute] FOREIGN KEY([AttributeID])
            REFERENCES [dbo].[GRS_Attribute] ([AttributeID])
    ALTER TABLE [dbo].[GRS_Entity_Attribute] CHECK CONSTRAINT [FK_GRS_Entity_Attribute_GRS_Attribute]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Entity_Attribute_GRS_Entity')
BEGIN
    ALTER TABLE [dbo].[GRS_Entity_Attribute] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Entity_Attribute_GRS_Entity] FOREIGN KEY([EntityID])
            REFERENCES [dbo].[GRS_Entity] ([EntityID])
    ALTER TABLE [dbo].[GRS_Entity_Attribute] CHECK CONSTRAINT [FK_GRS_Entity_Attribute_GRS_Entity]
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GRS_Sport_Attribute')
BEGIN

    CREATE TABLE [dbo].[GRS_Sport_Attribute](
        [SportAttributeID] [int] IDENTITY(1,1) NOT NULL,
        [SportID] int NOT NULL,
        [AttributeID] int NOT NULL,
        [AttributeName] [nvarchar](50) NOT NULL,
        [AttributeSetting] [nvarchar](1000) NULL,

        VersionAutoID bigint null,

        TSCreateDate datetime null,
        TSCreateUser [nvarchar](80) NOT NULL,
        TSModifyDate datetime null,
        TSModifyUser [nvarchar](80) NULL,
                
        CONSTRAINT [PK_GRS_Sport_Attribute_SportAttributeID] PRIMARY KEY CLUSTERED 
            (
                [SportAttributeID] ASC
            ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                        IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Sport_Attribute_GRS_Attribute')
BEGIN
    ALTER TABLE [dbo].[GRS_Sport_Attribute] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Sport_Attribute_GRS_Attribute] FOREIGN KEY([AttributeID])
            REFERENCES [dbo].[GRS_Attribute] ([AttributeID])
    ALTER TABLE [dbo].[GRS_Sport_Attribute] CHECK CONSTRAINT [FK_GRS_Sport_Attribute_GRS_Attribute]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = N'FK_GRS_Sport_Attribute_GRS_Sport')
BEGIN
    ALTER TABLE [dbo].[GRS_Sport_Attribute] WITH CHECK
        ADD CONSTRAINT [FK_GRS_Sport_Attribute_GRS_Sport] FOREIGN KEY([SportID])
            REFERENCES [dbo].[GRS_Sport] ([SportID])
    ALTER TABLE [dbo].[GRS_Sport_Attribute] CHECK CONSTRAINT [FK_GRS_Sport_Attribute_GRS_Sport]
END
GO


        