CREATE PROCEDURE [blog].[Category_GetAll]
AS
BEGIN
	SELECT	[Slug]
			, MAX([Name]) AS [Name]
			, COUNT(*) AS PostsCount
	FROM	[blog].[Category_View] c JOIN
			[blog].[PostCategory_View] pc ON pc.[CategorySlug] = c.[Slug]
	GROUP BY
			[Slug];
END;
