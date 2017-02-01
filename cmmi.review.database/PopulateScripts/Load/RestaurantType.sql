PRINT 'Setting restaurant types for database ' + DB_NAME() + ' on server ' + @@SERVERNAME

DECLARE @RType nvarchar(50)

SET @RType = 'American'
IF NOT EXISTS (Select Id FROM [dbo].[RestaurantType] WITH (NOLOCK) WHERE Name = @RType)
BEGIN
	INSERT INTO [dbo].[RestaurantType]
		(Name, Deleted)
	VALUES (@RType, 0)
END

SET @RType = 'Bar/Grill'
IF NOT EXISTS (Select Id FROM [dbo].[RestaurantType] WITH (NOLOCK) WHERE Name = @RType)
BEGIN
	INSERT INTO [dbo].[RestaurantType]
		(Name, Deleted)
	VALUES (@RType, 0)
END

SET @RType = 'Chinese'
IF NOT EXISTS (Select Id FROM [dbo].[RestaurantType] WITH (NOLOCK) WHERE Name = @RType)
BEGIN
	INSERT INTO [dbo].[RestaurantType]
		(Name, Deleted)
	VALUES (@RType, 0)
END

SET @RType = 'Italian'
IF NOT EXISTS (Select Id FROM [dbo].[RestaurantType] WITH (NOLOCK) WHERE Name = @RType)
BEGIN
	INSERT INTO [dbo].[RestaurantType]
		(Name, Deleted)
	VALUES (@RType, 0)
END

SET @RType = 'Casual'
IF NOT EXISTS (Select Id FROM [dbo].[RestaurantType] WITH (NOLOCK) WHERE Name = @RType)
BEGIN
	INSERT INTO [dbo].[RestaurantType]
		(Name, Deleted)
	VALUES (@RType, 0)
END