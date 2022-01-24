CREATE VIEW [banners].[SiteBanner_View]
AS
	WITH LatestSiteBannerEvent AS (
		SELECT	[SiteBannerId]
				, MAX([EventDate]) AS LatestEventDate
		FROM	[banners].[SiteBannerEvent]
		WHERE	[EventDate] <= SYSDATETIMEOFFSET()
		GROUP BY
				[SiteBannerId]
	)
	SELECT	sb.[Id]
			, sb.[ThemeId]
			, t.[Description] AS ThemeDescription
			, sb.[Lead]
			, sb.[Body]
			, sbe.[IsEnabled]
			, lsbe.LatestEventDate AS LastModifiedDate
	FROM	[banners].[SiteBanner] sb JOIN
			[dbo].[AlertTheme] t ON t.[Id] = sb.[ThemeId] JOIN
			[banners].[SiteBannerEvent] sbe ON sbe.[SiteBannerId] = sb.[Id] JOIN
			LatestSiteBannerEvent lsbe ON lsbe.[SiteBannerId] = sbe.[SiteBannerId]
				AND lsbe.LatestEventDate = sbe.[EventDate];
