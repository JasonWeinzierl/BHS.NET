CREATE PROCEDURE [blog].[_SetPostRevision]
	@slug VARCHAR(127),
	@title NVARCHAR(255),
	@contentMarkdown NVARCHAR(MAX),
	@filePath VARCHAR(255) NULL,
	@photosAlbumSlug VARCHAR(127) NULL,
	@authorId INT NULL
AS
BEGIN
	INSERT INTO
			[blog].[PostRevision]
			([PostSlug], [Title], [ContentMarkdown], [FilePath], [PhotosAlbumSlug], [AuthorId])
	VALUES	(@slug, @title, @contentMarkdown, @filePath, @photosAlbumSlug, @authorId);

	RETURN	SCOPE_IDENTITY();
END;
