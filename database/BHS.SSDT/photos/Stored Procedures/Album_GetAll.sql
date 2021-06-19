CREATE PROCEDURE [photos].[Album_GetAll]
AS
BEGIN
	SELECT	[Slug]
			, alb.[Name]
			, [Description]

			, [BannerPhotoId]
			, p.[Name] AS BannerPhotoName
			, p.[ImagePath] AS BannerPhotoImagePath
			, p.[DatePosted] AS BannerPhotoDatePosted
			, p.[AuthorId] AS BannerPhotoAuthorId

			, [BlogPostSlug]
			
			, alb.[AuthorId]
			, [ath].[DisplayName] AS AuthorDisplayName
			, [ath].[Name] AS AuthorName

	FROM	[photos].[Album_View] alb LEFT JOIN
			[dbo].[Author_View] ath ON ath.[Id] = alb.[AuthorId] LEFT JOIN
			[photos].[Photo_View] p ON p.[Id] = alb.[BannerPhotoId];
END
