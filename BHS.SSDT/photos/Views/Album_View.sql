CREATE VIEW [photos].[Album_View]
AS
	SELECT	[Slug]
			, [Name]
			, [Description]
			, [BannerPhotoId]
			, [BlogPostSlug]
			, [AuthorId]
	FROM	[photos].[Album];
