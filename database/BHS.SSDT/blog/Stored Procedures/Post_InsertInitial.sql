CREATE PROCEDURE [blog].[Post_InsertInitial]
	@slug VARCHAR(127),
	@title NVARCHAR(255),
	@contentMarkdown NVARCHAR(MAX),
	@filePath VARCHAR(255) NULL,
	@photosAlbumSlug VARCHAR(127) NULL,
	@authorId INT NULL
AS
BEGIN
	DECLARE @_inNestedTransaction BIT;

	BEGIN TRY
	
		IF	@@TRANCOUNT = 0
		BEGIN
			SET	@_inNestedTransaction = 0;
			BEGIN TRANSACTION;
		END
		ELSE
		BEGIN
			SET @_inNestedTransaction = 1;
		END;

		INSERT INTO
				[blog].[Post]
		VALUES	(@slug);

		
		DECLARE @_revisionId BIGINT;
		EXEC	@_revisionId = [blog].[_SetPostRevision] @slug, @title, @contentMarkdown, @filePath, @photosAlbumSlug, @authorId;

		EXEC	[blog].[PostRevision_GetById] @_revisionId;

		IF	@@TRANCOUNT > 0
			AND @_inNestedTransaction = 0
		BEGIN
			COMMIT TRANSACTION;
		END;

	END TRY
	BEGIN CATCH
		
		IF	@@TRANCOUNT > 0
			AND @_inNestedTransaction = 0
		BEGIN
			ROLLBACK TRANSACTION;
		END;

		THROW;

	END CATCH;
END;
