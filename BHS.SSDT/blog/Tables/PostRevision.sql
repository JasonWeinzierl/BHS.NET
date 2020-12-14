CREATE TABLE [blog].[PostRevision]
(
	[Id] INT IDENTITY(1,1) CONSTRAINT PK_PostRevision PRIMARY KEY,
    [DateCreated] DATETIMEOFFSET NOT NULL,
    [PostSlug] VARCHAR(127) NOT NULL,
    [Title] NVARCHAR (255) NOT NULL,
    [ContentMarkdown] NVARCHAR (MAX) NULL,
    [FilePath] VARCHAR (255) NULL,
    [PhotosAlbumId] INT NULL,
    [AuthorId] INT NULL,
    CONSTRAINT FK_PostRevision_Post_Slug FOREIGN KEY([PostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE CASCADE,
    CONSTRAINT FK_PostRevision_Author_AuthorId FOREIGN KEY (AuthorId) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_PostRevision_Album_PhotosAlbumId FOREIGN KEY (PhotosAlbumId) REFERENCES [photos].[Photo] ([Id]) ON DELETE SET NULL
);
