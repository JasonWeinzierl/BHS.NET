CREATE PROCEDURE [photos].[AlbumPhotos_GetBySlug]
	@slug VARCHAR(127)
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
			[photos].[Photo_View] p ON p.[Id] = alb.[BannerPhotoId]
	WHERE	[Slug] = @slug;
	
	SELECT	[Id]
			, [Name]
			, [ImagePath]
			, [DatePosted]
			, [AuthorId]
	FROM	[photos].[Photo_View] p JOIN
			[photos].[Exhibit_View] e ON e.[PhotoId] = p.[Id]
	WHERE	e.[AlbumSlug] = @slug;
END
