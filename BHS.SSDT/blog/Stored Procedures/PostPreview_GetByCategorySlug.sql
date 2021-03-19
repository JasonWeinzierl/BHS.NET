CREATE PROCEDURE [blog].[PostPreview_GetByCategorySlug]
(
	@categorySlug VARCHAR(127)
)
AS
BEGIN
	SELECT	[Slug]
			, [Title]
			, [ContentMarkdown] AS ContentPreview
			, [AuthorId]
			, a.[DisplayName] AS AuthorDisplayName
			, a.[Name] AS AuthorName
			, [DatePublished]
	FROM	[blog].[Post_View] p JOIN
			[blog].[PostCategory_View] pc ON pc.[PostSlug] = p.[Slug] LEFT JOIN
			[dbo].[Author_View] a ON a.[Id] = p.[AuthorId]
	WHERE	pc.[CategorySlug] = @categorySlug;
END
