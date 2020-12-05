CREATE TABLE [blog].[Post] (
    [Id] INT IDENTITY(1,1) CONSTRAINT PK_BlogPost PRIMARY KEY,
    [Title] NVARCHAR (255) NOT NULL,
    [BodyContent] NVARCHAR (MAX) NULL,
    [FilePath] VARCHAR (255) NULL,
    [PhotosAlbumId] INT NULL,
    [IsVisible] BIT CONSTRAINT DF_BlogPost_IsVisible DEFAULT ((1)) NOT NULL,
    [AuthorId] INT NULL, 
    [PublishDate] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_Post_Author_AuthorId FOREIGN KEY (AuthorId) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_Post_Album_PhotosAlbumId FOREIGN KEY (PhotosAlbumId) REFERENCES [photos].[Photo] ([Id]) ON DELETE SET NULL
);

