﻿CREATE PROCEDURE [photos].[Photo_GetById]
	@id INT
AS
BEGIN
	SELECT	[Id]
			, [Name]
			, [ImagePath]
			, [DatePosted]
			, [AuthorId]
			, [Description]
	FROM	[photos].[Photo_View]
	WHERE	[Id] = @id;
END