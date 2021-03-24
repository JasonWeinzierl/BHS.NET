CREATE PROCEDURE [blog].[PostPreviewCategory_Search]
(
	@searchText VARCHAR(255),
	@fromDate DateTimeOffset = NULL,
	@toDate DateTimeOffset = NULL
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
			[blog].[Category_View] c ON c.[Slug] = pc.[CategorySlug] LEFT JOIN
			[dbo].[Author_View] a ON a.[Id] = p.[AuthorId]
	WHERE	[ContentMarkdown] LIKE CONCAT('%', @searchText, '%')
			AND (@fromDate IS NULL OR @fromDate <= [DatePublished])
			AND (@toDate IS NULL OR @toDate > [DatePublished]);
END
