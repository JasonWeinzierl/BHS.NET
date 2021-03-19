CREATE PROCEDURE [blog].[PostPreview_Search]
(
	@searchText VARCHAR(255),
	@fromDate DateTimeOffset = NULL,
	@toDate DateTimeOffset = NULL
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
	FROM	[blog].[Post_View] v LEFT JOIN
			[dbo].[Author_View] a ON a.[Id] = v.[AuthorId]
	WHERE	[ContentMarkdown] LIKE CONCAT('%', @searchText, '%')
			AND (@fromDate IS NULL OR @fromDate <= [DatePublished])
			AND (@toDate IS NULL OR @toDate > [DatePublished]);
END
