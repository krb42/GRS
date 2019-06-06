/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create VersionAutoID_Sequence          </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VersionAutoID_Sequence]') AND type = 'SO')
CREATE SEQUENCE [dbo].[VersionAutoID_Sequence] 
    AS [bigint]
    START WITH 1
    INCREMENT BY 1
    MINVALUE 1
    NO MAXVALUE
    CACHE  3 
GO



