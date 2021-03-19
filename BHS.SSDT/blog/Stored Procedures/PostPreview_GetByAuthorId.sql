CREATE PROCEDURE [blog].[PostPreview_GetByAuthorId]
(
	@authorId INT
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
	FROM	[blog].[Post_View] v JOIN
			[dbo].[Author_View] a ON a.[Id] = v.[AuthorId]
	WHERE	[AuthorId] = @authorId;
END
