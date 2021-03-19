CREATE PROCEDURE [dbo].[Author_GetByUserName]
	@userName NVARCHAR(255)
AS
BEGIN
	SELECT	[Id]
			, [DisplayName]
			, [Name]
	FROM	[dbo].[Author_View]
	WHERE	[DisplayName] = @userName;
END
