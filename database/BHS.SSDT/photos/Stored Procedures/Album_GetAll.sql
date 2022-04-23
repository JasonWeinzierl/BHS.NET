CREATE PROCEDURE [photos].[Album_GetAll]
AS
BEGIN
	SELECT	[Slug]
			, alb.[Name]
			, alb.[Description]

			, [BannerPhotoId]
			, bp.[Name] AS BannerPhotoName
			, bp.[ImagePath] AS BannerPhotoImagePath
			, bp.[DatePosted] AS BannerPhotoDatePosted
			, bp.[AuthorId] AS BannerPhotoAuthorId
			, bp.[Description] AS BannerPhotoDescription

			, [BlogPostSlug]
			
			, alb.[AuthorId]
			, [ath].[DisplayName] AS AuthorDisplayName
			, [ath].[Name] AS AuthorName

	FROM	[photos].[Album_View] alb LEFT JOIN
			[dbo].[Author_View] ath ON ath.[Id] = alb.[AuthorId] LEFT JOIN
			[photos].[Photo_View] bp ON bp.[Id] = alb.[BannerPhotoId];
END
