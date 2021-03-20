CREATE PROCEDURE [blog].[Category_GetAll]
AS
BEGIN
	SELECT	[Slug]
			, [Name]
			, PostsCount
	FROM	[blog].[Category_View];
END;
