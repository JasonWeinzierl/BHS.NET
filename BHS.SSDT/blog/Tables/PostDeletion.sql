CREATE TABLE [blog].[PostDeletion]
(
	[PostSlug] VARCHAR(127) NOT NULL,
	[DateDeleted] DATETIMEOFFSET NOT NULL,
	CONSTRAINT PK_PostDeletion PRIMARY KEY ([PostSlug], [DateDeleted]),
	CONSTRAINT FK_PostDeletion_Post_Slug FOREIGN KEY ([PostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE CASCADE
);
