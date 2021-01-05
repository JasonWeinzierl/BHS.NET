CREATE PROCEDURE [photos].[Photo_GetByAlbumId]
	@albumId INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [ImagePath]
			, [DatePosted]
			, [AuthorId]
	FROM	[photos].[Photo] p JOIN
			[photos].[Exhibit] e ON e.[PhotoId] = p.[Id]
	WHERE	e.[AlbumId] = @albumId;
END
