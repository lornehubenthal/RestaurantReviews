CREATE TABLE [dbo].[Review]
(
	[Id] INT NOT NULL IDENTITY, 
    [RestaurantId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [Rating] INT NOT NULL, 
    [Comments] NVARCHAR(750) NOT NULL DEFAULT (''), 
    [Deleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Review_Restaurant] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant]([Id]), 
    CONSTRAINT [FK_Review_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    
)
GO

ALTER TABLE [dbo].[Review]
ADD CONSTRAINT PK_Yard_Queue PRIMARY KEY NONCLUSTERED (Id);  
GO

CREATE CLUSTERED INDEX  [IX_Review_UserId] ON [dbo].[Review] (UserId)
GO
