/*------------------------------------------------------------------------------------
   Project/Ticket#: GRS
   <Description>    Create General Access user  </Description>
   <Version>        1.0.0                         </version>
-------------------------------------------------------------------------------------*/

USE [$DatabaseName$]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* Create Login for User */
IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = 'grsuser')
BEGIN
    CREATE LOGIN grsuser 
    WITH PASSWORD = 'grsuser'
END
GO

/* Create the user for the specified login. */
IF NOT EXISTS(SELECT principal_id FROM sys.database_principals WHERE name = 'grsuser')
BEGIN
    CREATE USER grsuser FOR LOGIN grsuser
    ALTER USER grsuser WITH DEFAULT_SCHEMA = GRSData_Dev   
    EXEC sp_addrolemember 'db_owner', 'grsuser'
END
GO

/* Create Login for User */
IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = 'testuser')
BEGIN
    CREATE LOGIN testuser 
    WITH PASSWORD = 'testuser'
END
GO

/* Create the user for the specified login. */
IF NOT EXISTS(SELECT principal_id FROM sys.database_principals WHERE name = 'testuser')
BEGIN
    CREATE USER testuser FOR LOGIN testuser
    ALTER USER testuser WITH DEFAULT_SCHEMA = GRSData_Dev   
    EXEC sp_addrolemember 'db_owner', 'testuser'
END
GO



