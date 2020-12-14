CREATE PROCEDURE [blog].[Category_GetByPostSlug]
	@postSlug VARCHAR(127)
AS
BEGIN
	SELECT	[CategorySlug] AS Slug
			, [Name]
	FROM	[blog].[PostCategory_View] pc JOIN
			[blog].[Category] c ON c.[Slug] = pc.[CategorySlug]
	WHERE	[PostSlug] = @postSlug;
END
