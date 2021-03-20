CREATE PROCEDURE [blog].[Category_GetBySlug]
	@slug VARCHAR(127)
AS
BEGIN
	SELECT	[Slug]
			, [Name]
			, PostsCount
	FROM	[blog].[Category_View]
	WHERE	[Slug] = @slug;
END
