CREATE VIEW [blog].[PostCategory_View]
AS
	WITH LatestCategoryEvent AS (
		SELECT	[CategorySlug]
				, [PostSlug]
				, MAX([EventDate]) AS LatestEventDate
		FROM	[blog].[PostCategoryEvent]
		GROUP BY
				[CategorySlug]
				, [PostSlug]
	)
	SELECT	pce.[CategorySlug]
			, pce.[PostSlug]
	FROM	[blog].[PostCategoryEvent] pce JOIN
			LatestCategoryEvent lce ON lce.[CategorySlug] = pce.[CategorySlug]
				AND lce.[PostSlug] = pce.[PostSlug]
				AND lce.LatestEventDate = pce.[EventDate]
	WHERE	pce.[IsEnabled] = 1;
