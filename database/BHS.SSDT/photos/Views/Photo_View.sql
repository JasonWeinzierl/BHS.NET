CREATE VIEW [photos].[Photo_View]
AS
	SELECT	[Id]
			, [Name]
			, [ImagePath]
			, [DatePosted]
			, [AuthorId]
			, [Description]
	FROM	[photos].[Photo];
