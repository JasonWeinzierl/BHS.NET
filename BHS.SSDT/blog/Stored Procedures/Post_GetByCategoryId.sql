CREATE PROCEDURE [blog].[Post_GetByCategoryId]
	@categoryId INT
AS
BEGIN
	SELECT	[Id]
			, [Title]
			, [BodyContent]
			, [FilePath]
			, [PhotosAlbumId]
			, [IsVisible]
			, [AuthorId]
			, [PublishDate]
	FROM	[blog].[Post] p JOIN
			[blog].[PostCategory] pc ON pc.PostId = p.Id
	WHERE	pc.[CategoryId] = @categoryId;
END
