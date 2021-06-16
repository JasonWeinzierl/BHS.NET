CREATE PROCEDURE [blog].[PostRevision_Insert]
	@slug VARCHAR(127),
	@title NVARCHAR(255),
	@contentMarkdown NVARCHAR(MAX),
	@filePath VARCHAR(255) NULL,
	@photosAlbumSlug VARCHAR(127) NULL,
	@authorId INT NULL
AS
BEGIN
	DECLARE @_revisionId BIGINT;
	EXEC	@_revisionId = [blog].[_SetPostRevision]
			@slug,
			@title,
			@contentMarkdown,
			@filePath,
			@photosAlbumSlug,
			@authorId;

	EXEC	[blog].[PostRevision_GetById] @_revisionId;
END;
