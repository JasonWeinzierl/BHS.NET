CREATE PROCEDURE [blog].[Post_GetByAuthorId]
	@authorId INT
AS
BEGIN
	SELECT	[Id]
			, [Title]
			, [BodyContent]
			, [FilePath]
			, [PhotosAlbumId]
			, [IsVisible]
			, [AuthorId]
			, [PublishDate]
	FROM	[blog].[Post]
	WHERE	[AuthorId] = @authorId;
END
