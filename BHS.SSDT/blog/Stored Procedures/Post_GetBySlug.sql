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
	
	SELECT	[CategorySlug] AS Slug
			, [Name]
	FROM	[blog].[PostCategory_View] pc JOIN
			[blog].[Category_View] c ON c.[Slug] = pc.[CategorySlug]
	WHERE	[PostSlug] = @slug;
END
