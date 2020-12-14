CREATE VIEW [blog].[Post_View]
AS
	SELECT	latestRev.[PostSlug] AS Slug
			, [Title]
			, [ContentMarkdown]
			, [FilePath]
			, [PhotosAlbumId]
			, [AuthorId]
			, firstRev.[DatePublished] AS DatePublished
			, latestRev.[DatePublished] AS DateLastModified
	FROM	[blog].[PostLatestRevision_View] latestRev LEFT JOIN
			[blog].[PostDeletion_View] del ON del.[PostSlug] = latestRev.[PostSlug] LEFT JOIN
			[blog].[PostFirstRevisionDate_View] firstRev ON firstRev.[PostSlug] = latestRev.[PostSlug]
	WHERE	del.[LatestDateDeleted] IS NULL
				AND latestRev.[DatePublished] IS NOT NULL
			OR latestRev.[DatePublished] > del.[LatestDateDeleted];
