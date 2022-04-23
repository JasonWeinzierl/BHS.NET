CREATE TABLE [photos].[Photo] (
    [Id] INT IDENTITY(1,1) CONSTRAINT PK_PhotosPhoto PRIMARY KEY,
    [Name] NVARCHAR (255) NULL,
    [ImagePath] VARCHAR(MAX) NOT NULL,
    [DatePosted] DATETIMEOFFSET NOT NULL,
    [AuthorId] INT NULL,
    [Description] NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Photo_Author FOREIGN KEY (AuthorId) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL
);
