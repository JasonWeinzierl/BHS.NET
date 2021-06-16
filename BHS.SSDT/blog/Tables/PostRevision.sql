CREATE TABLE [blog].[PostRevision]
(
	[Id] INT IDENTITY(1,1) CONSTRAINT PK_PostRevision PRIMARY KEY,
    [DateCreated] DATETIMEOFFSET NOT NULL CONSTRAINT DF_PostRevision_DateCreated DEFAULT SYSDATETIMEOFFSET(),
    [PostSlug] VARCHAR(127) NOT NULL,

    [Title] NVARCHAR (255) NOT NULL,
    [ContentMarkdown] NVARCHAR (MAX) NOT NULL,
    [FilePath] VARCHAR (255) NULL,
    [PhotosAlbumSlug] VARCHAR(127) NULL,
    [AuthorId] INT NULL,

    CONSTRAINT FK_PostRevision_Post FOREIGN KEY([PostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE CASCADE,
    CONSTRAINT FK_PostRevision_Author FOREIGN KEY (AuthorId) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_PostRevision_Album FOREIGN KEY ([PhotosAlbumSlug]) REFERENCES [photos].[Album] ([Slug]) ON DELETE SET NULL
);
