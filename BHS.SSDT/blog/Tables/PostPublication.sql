CREATE TABLE [blog].[PostPublication]
(
	[RevisionId] INT NOT NULL,
	[DatePublished] DATETIMEOFFSET NOT NULL,
	CONSTRAINT PK_PostPublication PRIMARY KEY ([RevisionId], [DatePublished]),
	CONSTRAINT FK_PostPublication_PostRevision_RevisionId FOREIGN KEY ([RevisionId]) REFERENCES [blog].[PostRevision] ([Id]) ON DELETE CASCADE
);
