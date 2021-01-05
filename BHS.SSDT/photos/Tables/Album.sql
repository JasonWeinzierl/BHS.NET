CREATE TABLE [photos].[Album] (
    [Id] INT IDENTITY(1,1) CONSTRAINT PK_PhotosAlbum PRIMARY KEY,
    [Name] NVARCHAR (255)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    [BannerPhotoId] INT NULL,
    [BlogPostSlug] VARCHAR(127) NULL,
    [AuthorId] INT NULL,
    CONSTRAINT FK_Album_Author_AuthorId FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_Album_Post_BlogPostSlug FOREIGN KEY ([BlogPostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE SET NULL,
    CONSTRAINT FK_Album_Photo_BannerPhotoId FOREIGN KEY ([BannerPhotoId]) REFERENCES [photos].[Photo] ([Id]) ON DELETE SET NULL
);
