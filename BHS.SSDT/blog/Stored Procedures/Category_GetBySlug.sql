CREATE PROCEDURE [blog].[Category_GetBySlug]
	@slug VARCHAR(127)
AS
BEGIN
	SELECT	[Slug]
			, [Name]
	FROM	[blog].[Category]
	WHERE	[Slug] = @slug
END
