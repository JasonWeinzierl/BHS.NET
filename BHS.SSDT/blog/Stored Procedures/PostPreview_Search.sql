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
			, [ContentMarkdown]
			, [AuthorId]
			, [DatePublished]
	FROM	[blog].[Post_View]
	WHERE	[ContentMarkdown] LIKE CONCAT('%', @searchText, '%')
			AND (@fromDate IS NULL OR @fromDate <= [DatePublished])
			AND (@toDate IS NULL OR @toDate > [DatePublished]);
END
