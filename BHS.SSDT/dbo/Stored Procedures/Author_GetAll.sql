CREATE PROCEDURE [dbo].[Author_GetAll]
AS
BEGIN
	SELECT	[Id]
			, [DisplayName]
			, [Name]
	FROM	[dbo].[Author];
END
