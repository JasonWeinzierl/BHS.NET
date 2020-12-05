CREATE PROCEDURE [dbo].[Author_GetAll]
	@doIncludeHidden BIT = 0
AS
BEGIN
	SELECT	[Id]
			, [DisplayName]
			, [Name]
			, [IsVisible]
	FROM	[dbo].[Author]
	WHERE	[IsVisible] = 1
			OR @doIncludeHidden = 1;
END
