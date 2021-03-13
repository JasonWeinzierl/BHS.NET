CREATE PROCEDURE [blog].[Category_GetBySlug]
	@slug VARCHAR(127)
AS
BEGIN
	SELECT	[Slug]
			, [Name]
	FROM	[blog].[Category_View]
	WHERE	[Slug] = @slug
END
