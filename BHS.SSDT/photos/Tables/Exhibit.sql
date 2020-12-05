CREATE TABLE [photos].[Exhibit] (
    [AlbumId] INT NOT NULL,
    [PhotoId] INT NOT NULL,
    CONSTRAINT PK_Exhibit PRIMARY KEY (AlbumId, PhotoId),
    CONSTRAINT FK_Exhibit_Album_AlbumId FOREIGN KEY (AlbumId) REFERENCES [photos].[Album] ([Id]) ON DELETE CASCADE,
    CONSTRAINT FK_Exhibit_Photo_PhotoId FOREIGN KEY (PhotoId) REFERENCES [photos].[Photo] ([Id]) ON DELETE CASCADE
);
