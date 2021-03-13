CREATE PROCEDURE [blog].[PostPreview_GetByCategorySlug]
(
	@categorySlug VARCHAR(127)
)
AS
BEGIN
	SELECT	[Slug]
			, [Title]
			, [ContentMarkdown] AS ContentPreview
			, [AuthorId]
			, [DatePublished]
	FROM	[blog].[Post_View] p JOIN
			[blog].[PostCategory_View] pc ON pc.[PostSlug] = p.[Slug]
	WHERE	pc.[CategorySlug] = @categorySlug;
END
