﻿CREATE TABLE [dbo].[Author] (
    [Id] INT IDENTITY (1, 1) NOT NULL CONSTRAINT PK_Author PRIMARY KEY,
    [DisplayName] NVARCHAR (255) CONSTRAINT [UK_Author_DisplayName] UNIQUE NOT NULL,
    [Name] NVARCHAR (255) NULL
);