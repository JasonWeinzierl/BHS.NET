CREATE PROCEDURE [photos].[Album_GetById]
	@id INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [Description]
			, [BannerPhotoId]
			, [BlogPostId]
			, [IsVisible]
			, [DateUpdated]
			, [AuthorId]
	FROM	[photos].[Album]
	WHERE	[Id] = @id;
END
