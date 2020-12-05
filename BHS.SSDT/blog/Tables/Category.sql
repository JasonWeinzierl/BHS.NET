CREATE TABLE [blog].[Category] (
    [Id]         INT           IDENTITY(1,1) CONSTRAINT PK_BlogCategory PRIMARY KEY,
    [Name]       NVARCHAR (50) NULL,
    [IsVisible]  BIT           NOT NULL
);
