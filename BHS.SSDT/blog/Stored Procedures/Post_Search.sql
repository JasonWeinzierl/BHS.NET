CREATE PROCEDURE [blog].[Post_Search]
	@searchText VARCHAR(255),
	@fromDate DateTimeOffset = NULL,
	@toDate DateTimeOffset = NULL
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
	FROM	[blog].[Post]
	WHERE	[BodyContent] LIKE CONCAT('%', @searchText, '%')
			AND (@FromDate IS NULL OR @fromDate <= [PublishDate])
			AND (@ToDate IS NULL OR @toDate > [PublishDate]);
END
