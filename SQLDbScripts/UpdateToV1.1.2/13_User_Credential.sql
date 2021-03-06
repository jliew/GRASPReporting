BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION

USE [GRASP]

GO
ALTER TABLE dbo.User_Credential ADD
	UserResponseFilter nvarchar(MAX) NULL
GO
ALTER TABLE dbo.User_Credential SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.User_Credential', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.User_Credential', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.User_Credential', 'Object', 'CONTROL') as Contr_Per 