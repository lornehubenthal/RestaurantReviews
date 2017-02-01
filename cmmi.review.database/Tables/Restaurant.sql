CREATE TABLE [dbo].[Restaurant]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL, 
    [StreetAddress1] NVARCHAR(100) NULL, 
    [StreetAddress2] NVARCHAR(100) NULL, 
    [City] NVARCHAR(50) NOT NULL, 
    [State] NVARCHAR(2) NOT NULL, 
    [Zip] NVARCHAR(10) NOT NULL, 
    [Type] INT NULL, 
    [Deleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Restaurant_RestaurantType] FOREIGN KEY ([Type]) REFERENCES [RestaurantType]([Id])
)
GO

CREATE NONCLUSTERED INDEX  [IX_Restaurant_City] ON [dbo].[Restaurant] (City)
GO