CREATE TABLE [dbo].[Author] (
    [Id]          INT                IDENTITY (1, 1) NOT NULL,
    [DisplayName] NVARCHAR (255)      CONSTRAINT [UK_Author_DisplayName] UNIQUE NOT NULL,
    [Name]        NVARCHAR (255)      NULL,
    [IsVisible]     BIT                CONSTRAINT [DF_Author_IsVisible] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED ([Id] ASC)
);
