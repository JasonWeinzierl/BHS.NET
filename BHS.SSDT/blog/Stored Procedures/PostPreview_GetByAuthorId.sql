CREATE PROCEDURE [blog].[PostPreview_GetByAuthorId]
(
	@authorId INT
)
AS
BEGIN
	SELECT	[Slug]
			, [Title]
			, [ContentMarkdown]
			, [AuthorId]
			, [DatePublished]
	FROM	[blog].[Post_View]
	WHERE	[AuthorId] = @authorId;
END
