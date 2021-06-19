CREATE FUNCTION [blog].[GetPostContentPreview]
(
	@contentMarkdown NVARCHAR(MAX),
    @searchText NVARCHAR(4000) = NULL
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @previewIndex INT;
    SELECT  @previewIndex = CHARINDEX(@searchText, @contentMarkdown) - 25;

    RETURN  (CASE
                WHEN @searchText IS NULL
                THEN CONCAT(LEFT(@contentMarkdown, 135), NCHAR(8230))
                WHEN @previewIndex <= 0
                THEN CONCAT(LEFT(@contentMarkdown, 135), NCHAR(8230))
                ELSE CONCAT(NCHAR(8230), SUBSTRING(@contentMarkdown, @previewIndex, 135), NCHAR(8230))
            END);
END;
