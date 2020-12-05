CREATE PROCEDURE [blog].[Post_GetById]
(
	@id [INT]
)
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
	WHERE	[Id] = @id;
END
