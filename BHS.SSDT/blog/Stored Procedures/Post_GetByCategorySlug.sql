CREATE PROCEDURE [blog].[Post_GetByCategorySlug]
(
	@categorySlug INT
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
	FROM	[blog].[Post_View] p JOIN
			[blog].[PostCategory_View] pc ON pc.[PostSlug] = p.[Slug]
	WHERE	pc.[CategorySlug] = @categorySlug;
END
