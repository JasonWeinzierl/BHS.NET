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
			, a.[DisplayName] AS AuthorDisplayName
			, a.[Name] AS AuthorName
			, [DatePublished]
			, [DateLastModified]
	FROM	[blog].[Post_View] v LEFT JOIN
			[dbo].[Author_View] a ON a.[Id] = v.[AuthorId]
	WHERE	[Slug] = @slug;
	
	SELECT	[CategorySlug] AS Slug
			, [Name]
			, PostsCount
	FROM	[blog].[PostCategory_View] pc JOIN
			[blog].[Category_View] c ON c.[Slug] = pc.[CategorySlug]
	WHERE	[PostSlug] = @slug;
END
