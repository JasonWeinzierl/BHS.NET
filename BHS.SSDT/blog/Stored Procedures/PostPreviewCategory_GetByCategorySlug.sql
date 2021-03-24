CREATE PROCEDURE [blog].[PostPreviewCategory_GetByCategorySlug]
	@categorySlug VARCHAR(127)
AS
BEGIN
	WITH posts AS (
		SELECT	[PostSlug]
		FROM	[blog].[PostCategory_View]
		WHERE	[CategorySlug] = @categorySlug
	)
	SELECT	p.[Slug]
			, [Title]
			, [blog].[GetPostContentPreview]([ContentMarkdown]) AS ContentPreview

			, [AuthorId]
			, a.[DisplayName] AS AuthorDisplayName
			, a.[Name] AS AuthorName

			, pc.[CategorySlug]
			, c.[Name] AS CategoryName

			, [DatePublished]

	FROM	[blog].[Post_View] p JOIN
			posts cte ON cte.[PostSlug] = p.[Slug] JOIN
			[blog].[PostCategory_View] pc ON pc.[PostSlug] = p.[Slug] JOIN
			[blog].[Category_View] c ON c.[Slug] = pc.[CategorySlug] LEFT JOIN
			[dbo].[Author_View] a ON a.[Id] = p.[AuthorId];
END
