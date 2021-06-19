CREATE PROCEDURE [leadership].[Director_GetCurrent]
	@startingYear INT
AS
BEGIN
	SELECT	[Name]
			, [Year]
	FROM	[leadership].[Director_View]
	WHERE	[Year] >= @startingYear
	ORDER BY
			[Year] ASC;
END;
