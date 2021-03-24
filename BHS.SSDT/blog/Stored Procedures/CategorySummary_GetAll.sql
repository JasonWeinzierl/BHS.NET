CREATE PROCEDURE [blog].[CategorySummary_GetAll]
AS
BEGIN
	WITH CategoryPostCount AS (
		SELECT	[Slug]
				, COUNT(*) AS PostsCount
		FROM	[blog].[Category] c JOIN
				[blog].[PostCategory_View] pc ON pc.[CategorySlug] = c.[Slug]
		GROUP BY
				[Slug]
	)
	SELECT	c.[Slug]
			, [Name]
			, ISNULL(cpc.PostsCount, 0) AS PostsCount
	FROM	[blog].[Category_View] c LEFT JOIN
			CategoryPostCount cpc ON cpc.[Slug] = c.[Slug];
END;
