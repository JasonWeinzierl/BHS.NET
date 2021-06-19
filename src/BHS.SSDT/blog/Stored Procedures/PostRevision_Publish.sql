CREATE PROCEDURE [blog].[PostRevision_Publish]
	@revisionId INT,
	@datePublished DATETIMEOFFSET
AS
BEGIN
	INSERT INTO
			[blog].[PostPublication]
			([RevisionId], [DatePublished])
	VALUES	(@revisionId, @datePublished);

	EXEC	[blog].[PostRevision_GetById] @revisionId;
END;
