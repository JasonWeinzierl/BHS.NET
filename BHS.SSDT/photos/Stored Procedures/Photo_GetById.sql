CREATE PROCEDURE [photos].[Photo_GetById]
	@id INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [ImagePath]
			, [IsVisible]
			, [DatePosted]
			, [AuthorId]
	FROM	[photos].[Photo]
	WHERE	[Id] = @id;
END
