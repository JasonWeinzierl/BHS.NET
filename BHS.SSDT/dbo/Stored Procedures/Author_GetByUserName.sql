CREATE PROCEDURE [dbo].[Author_GetByUserName]
	@userName NVARCHAR(255)
AS
BEGIN
	SELECT	[Id]
			, [DisplayName]
			, [Name]
			, [IsVisible]
	FROM	[dbo].[Author]
	WHERE	[DisplayName] = @userName;
END
