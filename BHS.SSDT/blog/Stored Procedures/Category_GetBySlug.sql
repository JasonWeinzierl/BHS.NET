CREATE PROCEDURE [blog].[Category_GetBySlug]
	@slug VARCHAR(127)
AS
BEGIN
	SELECT	[Slug]
			, MAX([Name]) AS [Name]
			, COUNT(*) AS PostsCount
	FROM	[blog].[Category_View] c JOIN
			[blog].[PostCategory_View] pc ON pc.[CategorySlug] = c.[Slug]
	WHERE	[Slug] = @slug
	GROUP BY
			[Slug];
END
