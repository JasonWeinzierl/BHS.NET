CREATE VIEW [blog].[PostFirstRevisionDate_View]
AS
	WITH FirstPublicationByRevision AS (
		SELECT	[RevisionId]
				, MIN([DatePublished]) AS FirstRevisionDatePublished
		FROM	[blog].[PostPublication]
		GROUP BY
				[RevisionId]
	)
	SELECT	[PostSlug]
			, MIN(pub.FirstRevisionDatePublished) AS DatePublished
	FROM	[blog].[PostRevision] rev JOIN
			FirstPublicationByRevision pub ON pub.[RevisionId] = rev.[Id]
	GROUP BY
			[PostSlug]
