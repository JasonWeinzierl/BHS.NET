CREATE PROCEDURE [blog].[PostRevision_GetById]
	@id INT
AS
BEGIN
	SELECT	rev.[Id]
			, [PostSlug]
			, [Title]
			, [ContentMarkdown]
			, [FilePath]
			, [PhotosAlbumSlug]

			, [AuthorId]
			, a.[DisplayName] AS AuthorDisplayName
			, a.[Name] AS AuthorName

			, [DateLastPublished]
	FROM	[blog].[PostRevision_View] rev LEFT JOIN
			[dbo].[Author_View] a ON a.[Id] = rev.[AuthorId]
	WHERE	rev.[Id] = @id;
END;
