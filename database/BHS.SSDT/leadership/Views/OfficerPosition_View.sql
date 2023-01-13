CREATE VIEW [leadership].[OfficerPosition_View]
AS
	WITH LatestPosition AS (
		SELECT	[PositionId]
				, MAX([DateStarted]) AS DateStarted
		FROM	[leadership].[OfficerPositionStart]
		GROUP BY
				[PositionId]
	)
	SELECT	p.[Title]
			, o.[Name]
			, p.[SortOrder]
            , ops.[DateStarted]
	FROM	[leadership].[OfficerPositionStart] ops JOIN
			LatestPosition latest ON latest.[PositionId] = ops.[PositionId]
				AND latest.DateStarted = ops.[DateStarted] JOIN
			[leadership].[Officer] o ON o.[Id] = ops.[OfficerId] JOIN
			[leadership].[Position] p ON p.[Id] = ops.[PositionId];
