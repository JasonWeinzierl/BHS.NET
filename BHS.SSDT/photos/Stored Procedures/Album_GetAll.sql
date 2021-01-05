CREATE PROCEDURE [photos].[Album_GetAll]
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [Description]
			, [BannerPhotoId]
			, [BlogPostSlug]
			, [AuthorId]
	FROM	[photos].[Album];
END
