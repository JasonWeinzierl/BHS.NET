CREATE PROCEDURE [blog].[Post_GetBySlug]
(
	@slug VARCHAR(127)
)
AS
BEGIN
	SELECT	[Slug]
			, [Title]
			, [ContentMarkdown]
			, [FilePath]
			, [PhotosAlbumId]
			, [AuthorId]
			, [DatePublished]
			, [DateLastModified]
	FROM	[blog].[Post_View]
	WHERE	[Slug] = @slug;
END
