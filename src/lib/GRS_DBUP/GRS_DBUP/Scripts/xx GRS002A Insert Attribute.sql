/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Insert initial for GRS_Attribute records            </Description>
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
    INSERT INTO GRS_User (UserName, UserCode, Deleted, TSCreateDate, TSCreateUser)
    VALUES ('Admin', 'ADMIN', 0, GETDATE(), 'Admin');
END


DECLARE @AdminUserID int;
SELECT @AdminUserID = UserID FROM GRS_User WHERE UserName = 'Admin';

EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entities', null, null
EXEC pAttribute_Insert @AdminUserID, 'Sport', 'Sports', null, null


DECLARE @ParentID int;
SELECT @ParentID = AttributeID FROM GRS_Attribute WHERE AttributeType = 'Entity' AND AttributeName = 'Entities';

EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Nominations', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Gold', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Silver', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Bronze', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Handicap_Gold', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Handicap_Silver', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Entity', 'Entity_Handicap_Bronze', null, @ParentID


SELECT @ParentID = AttributeID FROM GRS_Attribute WHERE AttributeType = 'Sport' AND AttributeName = 'Sports';

EXEC pAttribute_Insert @AdminUserID, 'Sport', 'Sport_Events', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Sport', 'Sport_Nominations', null, @ParentID
EXEC pAttribute_Insert @AdminUserID, 'Sport', 'Sport_Participants', null, @ParentID


