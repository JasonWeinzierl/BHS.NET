CREATE FUNCTION [blog].[GetPostContentPreview]
(
	@contentMarkdown NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    -- TODO: markdown entities should be stripped except newlines.
    --   search text should be in center of hook if applicable and marked.
    --   ellipses should be on bounds only where relevant.
	RETURN CONCAT(LEFT(@contentMarkdown, 135), NCHAR(8230))
END
