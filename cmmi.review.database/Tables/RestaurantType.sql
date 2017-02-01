CREATE TABLE [dbo].[RestaurantType]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Deleted] BIT NOT NULL DEFAULT 0
)
