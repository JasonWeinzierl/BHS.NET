CREATE VIEW [blog].[PostLatestRevision_View]
AS
	WITH LatestPublicationByRevision AS (
		SELECT	[RevisionId]
				, MAX([DatePublished]) AS LatestRevisionDatePublished
		FROM	[blog].[PostPublication]
		GROUP BY
				[RevisionId]
	),
	LatestRevisionDateByPost AS (
		SELECT	[PostSlug]
				, MAX(pub.LatestRevisionDatePublished) AS LatestPostDatePublished
		FROM	[blog].[PostRevision] rev JOIN
				LatestPublicationByRevision pub ON pub.[RevisionId] = rev.[Id]
		GROUP BY
				[PostSlug]
	)
	SELECT	rev.[PostSlug]
			, [Title]
			, [ContentMarkdown]
			, [FilePath]
			, [PhotosAlbumId]
			, [AuthorId]
			, [DatePublished]
			, [RevisionId]
	FROM	[blog].[PostRevision] rev JOIN
			[blog].[PostPublication] pub ON pub.[RevisionId] = rev.[Id] JOIN
			LatestRevisionDateByPost revDate ON revDate.LatestPostDatePublished = pub.[DatePublished]
				AND revDate.[PostSlug] = rev.[PostSlug];
