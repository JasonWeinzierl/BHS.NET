CREATE PROCEDURE [photos].[Album_GetAll]
	@doIncludeHidden BIT = 0
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
	WHERE	[IsVisible] = 1
			OR @doIncludeHidden = 1;
END
