CREATE TABLE [photos].[Exhibit] (
    [AlbumSlug] VARCHAR(127) NOT NULL,
    [PhotoId] INT NOT NULL,
    CONSTRAINT PK_Exhibit PRIMARY KEY ([AlbumSlug], [PhotoId]),
    CONSTRAINT FK_Exhibit_Album FOREIGN KEY ([AlbumSlug]) REFERENCES [photos].[Album] ([Slug]) ON DELETE CASCADE,
    CONSTRAINT FK_Exhibit_Photo FOREIGN KEY (PhotoId) REFERENCES [photos].[Photo] ([Id]) ON DELETE CASCADE
);
