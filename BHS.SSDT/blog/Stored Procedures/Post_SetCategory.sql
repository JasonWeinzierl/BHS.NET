CREATE PROCEDURE [blog].[Post_SetCategory]
	@postSlug VARCHAR(127),
	@categorySlug VARCHAR(127),
	@isEnabled BIT
AS
BEGIN
	INSERT INTO
			[blog].[PostCategoryEvent]
			([CategorySlug], [PostSlug], [IsEnabled])
	VALUES	(@categorySlug, @postSlug, @isEnabled);

	EXEC	[blog].[Post_GetBySlug] @postSlug;
END;
