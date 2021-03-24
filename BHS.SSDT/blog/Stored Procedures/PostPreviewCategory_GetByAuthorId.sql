CREATE PROCEDURE [blog].[PostPreviewCategory_GetByAuthorId]
(
	@authorId INT
)
AS
BEGIN
	SELECT	p.[Slug]
			, [Title]
			, [blog].[GetPostContentPreview]([ContentMarkdown]) AS ContentPreview

			, [AuthorId]
			, a.[DisplayName] AS AuthorDisplayName
			, a.[Name] AS AuthorName

			, pc.[CategorySlug]
			, c.[Name] AS CategoryName

			, [DatePublished]

	FROM	[blog].[Post_View] p LEFT JOIN
			[blog].[PostCategory_View] pc ON pc.[PostSlug] = p.[Slug] JOIN
			[blog].[Category_View] c ON c.[Slug] = pc.[CategorySlug] JOIN
			[dbo].[Author_View] a ON a.[Id] = p.[AuthorId]
	WHERE	[AuthorId] = @authorId;
END
