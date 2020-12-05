CREATE PROCEDURE [blog].[Category_GetByPostId]
	@postId INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [IsVisible]
	FROM	[blog].[Category] c JOIN
			[blog].[PostCategory] pc ON pc.CategoryId = c.Id
	WHERE	pc.[PostId] = @postId;
END
