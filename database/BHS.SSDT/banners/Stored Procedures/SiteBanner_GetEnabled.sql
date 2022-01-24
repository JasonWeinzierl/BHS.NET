CREATE PROCEDURE [banners].[SiteBanner_GetEnabled]
AS
BEGIN
	SELECT	[Id]
			, [ThemeId]
			, [Lead]
			, [Body]
	FROM	[banners].[SiteBanner_View]
	WHERE	[IsEnabled] = 1;
END
