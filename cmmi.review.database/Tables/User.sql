CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY, 
	[FirstName] NVARCHAR(50) NOT NULL DEFAULT ('') , 
	[LastName] NVARCHAR(50) NOT NULL DEFAULT ('') , 
    [Username] NVARCHAR(50) NOT NULL , 
    [PasswordHash] NVARCHAR(128) NOT NULL DEFAULT (''), 
    [PasswordSalt] NVARCHAR(88) NOT NULL DEFAULT (''), 
    [RememberMe] BIT NOT NULL DEFAULT 0, 
    [Locked] BIT NOT NULL DEFAULT 0, 
    [ForcePasswordChange] BIT NOT NULL DEFAULT  0, 
    [Deleted] BIT NOT NULL DEFAULT 0 
)
GO

ALTER TABLE [dbo].[User]   
ADD CONSTRAINT PK_Authentication_User PRIMARY KEY CLUSTERED (Id);  
GO

CREATE UNIQUE INDEX [IX_User_Username] ON [dbo].[User] (Username)
GO