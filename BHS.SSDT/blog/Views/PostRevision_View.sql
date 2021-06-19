CREATE VIEW [blog].[PostRevision_View]
AS
	WITH PostRevisionPublicationDates AS (
		SELECT	rev.[Id] AS RevisionId
				, MAX(pub.[DatePublished]) AS DateLastPublished
		FROM	[blog].[PostRevision] rev JOIN
				[blog].[PostPublication] pub ON pub.[RevisionId] = rev.[Id]
		GROUP BY
				rev.[Id]
	)
	SELECT	rev.[Id]
			, rev.[PostSlug]
			, rev.[Title]
			, rev.[ContentMarkdown]
			, rev.[FilePath]
			, rev.[PhotosAlbumSlug]
			, rev.[AuthorId]
			, dates.DateLastPublished
	FROM	[blog].[PostRevision] rev LEFT JOIN
			PostRevisionPublicationDates dates ON dates.RevisionId = rev.[Id];
