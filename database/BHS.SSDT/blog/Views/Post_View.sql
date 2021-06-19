CREATE VIEW [blog].[Post_View]
AS
	WITH PostDates AS (
		SELECT	[PostSlug]
				, MIN(pub.[DatePublished]) AS DatePublished
				, MAX(pub.[DatePublished]) AS DateLastModified
		FROM	[blog].[PostPublication] pub JOIN
				[blog].[PostRevision] rev ON rev.[Id] = pub.[RevisionId]
		GROUP BY
				[PostSlug]
	),
	PostLatestRevision AS (
		SELECT	rev.[PostSlug]
				, [Title]
				, [ContentMarkdown]
				, [FilePath]
				, [PhotosAlbumSlug]
				, [AuthorId]
				, pub.[DatePublished]
				, [RevisionId]
		FROM	[blog].[PostRevision] rev JOIN
				[blog].[PostPublication] pub ON pub.[RevisionId] = rev.[Id] JOIN
				PostDates dates ON dates.DateLastModified = pub.[DatePublished]
					AND dates.[PostSlug] = rev.[PostSlug]
	),
	PostDeletion AS (
		SELECT	[PostSlug]
				, MAX([DateDeleted]) AS LatestDateDeleted
		FROM	[blog].[PostDeletion]
		GROUP BY
				[PostSlug]
	)
	SELECT	latestRev.[PostSlug] AS Slug
			, [Title]
			, [ContentMarkdown]
			, [FilePath]
			, [PhotosAlbumSlug]
			, [AuthorId]
			, dates.[DatePublished]
			, dates.[DateLastModified]
	FROM	[blog].[Post] p JOIN
			PostLatestRevision latestRev ON latestRev.[PostSlug] = p.[Slug] LEFT JOIN
			PostDeletion del ON del.[PostSlug] = latestRev.[PostSlug] LEFT JOIN
			PostDates dates ON dates.[PostSlug] = latestRev.[PostSlug]
	WHERE	del.[LatestDateDeleted] IS NULL
				AND latestRev.[DatePublished] IS NOT NULL
			OR latestRev.[DatePublished] > del.[LatestDateDeleted];
