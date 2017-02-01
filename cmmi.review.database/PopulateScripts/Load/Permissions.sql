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
