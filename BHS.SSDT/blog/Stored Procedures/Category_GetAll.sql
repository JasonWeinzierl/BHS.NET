CREATE PROCEDURE [blog].[Category_GetAll]
AS
BEGIN
	SELECT	[Slug]
			, [Name]
	FROM	[blog].[Category_View];
END;
