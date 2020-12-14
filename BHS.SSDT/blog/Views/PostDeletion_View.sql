CREATE VIEW [blog].[PostDeletion_View]
AS
	SELECT	[PostSlug]
			, MAX([DateDeleted]) AS [LatestDateDeleted]
	FROM	[blog].[PostDeletion]
	GROUP BY
			[PostSlug];
