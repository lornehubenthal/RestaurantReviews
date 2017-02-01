/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

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

DECLARE @User nvarchar(50)

PRINT 'Setting permissions for database ' + DB_NAME() + ' on server ' + @@SERVERNAME

IF (@@SERVERNAME LIKE 'CSTECH%')
BEGIN
	SET @User = 'CSTECH-DEV01\appuser'
END
ELSE
BEGIN
	SET @User = 'trmrtappuser'
END

DECLARE @SQL nvarchar(4000)

		 SET @SQL = N'IF EXISTS (SELECT * FROM sysusers WHERE Name = ''' + @User + ''' ) DROP USER [' + @User + ']'

	SET @SQL = @SQL + N'; CREATE USER [' +  @User + '] FOR LOGIN [' + @User + '] WITH DEFAULT_SCHEMA=[dbo]'

	SET @SQL = @SQL +  N'; ALTER ROLE [db_datareader] ADD MEMBER [' +  @User + ']'

	SET @SQL = @SQL + N'; ALTER ROLE [db_datawriter] ADD MEMBER [' +  @User + ']'
	EXECUTE sp_executesql @SQL;
GO

GO
