CREATE TABLE [photos].[Album] (
    [Id] INT IDENTITY(1,1) CONSTRAINT PK_PhotosAlbum PRIMARY KEY,
    [Name] NVARCHAR (255)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    [BannerPhotoId] INT NULL,
    [BlogPostSlug] VARCHAR(127) NULL,
    [IsVisible] BIT CONSTRAINT DF_PhotosAlbum_IsVisible DEFAULT ((1)) NOT NULL,
    [DateUpdated] DATETIMEOFFSET CONSTRAINT DF_PhotosAlbum_DateUpdated DEFAULT (getutcdate()) NOT NULL,
    [AuthorId] INT NULL,
    CONSTRAINT FK_Album_Author_AuthorId FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_Album_Post_BlogPostSlug FOREIGN KEY ([BlogPostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE SET NULL,
    CONSTRAINT FK_Album_Photo_BannerPhotoId FOREIGN KEY ([BannerPhotoId]) REFERENCES [photos].[Photo] ([Id]) ON DELETE SET NULL
);
