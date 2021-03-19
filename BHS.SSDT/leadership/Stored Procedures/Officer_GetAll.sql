CREATE PROCEDURE [leadership].[Officer_GetAll]
AS
BEGIN
	SELECT	[Title]
			, [Name]
			, [SortOrder]
	FROM	[leadership].[OfficerPosition_View]
	ORDER BY
			[SortOrder] ASC;
END;
