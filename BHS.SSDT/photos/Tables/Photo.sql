CREATE TABLE [photos].[Photo] (
    [Id]          INT IDENTITY(1,1) CONSTRAINT PK_PhotosPhoto PRIMARY KEY,
    [Name]        NVARCHAR (255)  NULL,
    [ImagePath]        VARCHAR (MAX)  NULL,
    [IsVisible]     BIT CONSTRAINT DF_PhotosPhoto_IsVisible DEFAULT ((1)) NOT NULL,
    [DatePosted] DATETIMEOFFSET NULL,
    [AuthorId]           INT            NULL,
    CONSTRAINT FK_Photo_Author_AuthorId FOREIGN KEY (AuthorId) REFERENCES [dbo].[Author] ([Id]) ON DELETE SET NULL
);
