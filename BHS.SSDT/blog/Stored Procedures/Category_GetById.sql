CREATE PROCEDURE [blog].[Category_GetById]
	@id INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [IsVisible]
	FROM	[blog].[Category]
	WHERE	[Id] = @id
END
