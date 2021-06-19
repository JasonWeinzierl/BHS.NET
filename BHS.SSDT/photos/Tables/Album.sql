CREATE TABLE [photos].[Album] (
    [Slug] VARCHAR(127) NOT NULL CONSTRAINT PK_Album PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [BannerPhotoId] INT NULL,
    [BlogPostSlug] VARCHAR(127) NULL,
    [AuthorId] INT NULL,
    CONSTRAINT CK_Album_Slug_Alphanumeric CHECK ([Slug] NOT LIKE '%[^A-Z0-9-]%'),
    CONSTRAINT FK_Album_Author FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_Album_Post FOREIGN KEY ([BlogPostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE SET NULL ON UPDATE CASCADE,
    CONSTRAINT FK_Album_Photo FOREIGN KEY ([BannerPhotoId]) REFERENCES [photos].[Photo] ([Id]) ON DELETE SET NULL
);
