CREATE PROCEDURE [photos].[Album_GetById]
	@id INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [Description]
			, [BannerPhotoId]
			, [BlogPostSlug]
			, [AuthorId]
	FROM	[photos].[Album]
	WHERE	[Id] = @id;
END
