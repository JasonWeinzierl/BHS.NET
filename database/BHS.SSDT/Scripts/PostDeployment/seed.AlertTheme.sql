MERGE
	[dbo].[AlertTheme] AS TARGET
USING
	(
		VALUES
		(0, 'None'),
		(1, 'Primary'),
		(2, 'Secondary'),
		(3, 'Success'),
		(4, 'Danger'),
		(5, 'Warning'),
		(6, 'Info')
	)
	AS SOURCE
		([AlertThemeId], [Description])
ON
	TARGET.[Id] = SOURCE.[AlertThemeId]
WHEN NOT MATCHED BY TARGET THEN
INSERT
	(
		[Id],
		[Description]
	)
	VALUES
	(
		SOURCE.[AlertThemeId],
		SOURCE.[Description]
	)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET
		[Description] = SOURCE.[Description];
