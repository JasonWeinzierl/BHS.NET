CREATE TABLE [blog].[PostCategoryEvent] (
    [CategorySlug] VARCHAR(127) NOT NULL,
    [EventDate] DATETIMEOFFSET NOT NULL CONSTRAINT DF_PostCategoryEvent_EventDate DEFAULT SYSDATETIMEOFFSET(),
    [PostSlug] VARCHAR(127) NOT NULL,

    [IsEnabled] BIT NOT NULL CONSTRAINT DF_PostCategory_IsEnabled DEFAULT ((1)),

    CONSTRAINT PK_PostCategoryEvent PRIMARY KEY ([CategorySlug], [PostSlug], [EventDate]),
    CONSTRAINT FK_PostCategoryEvent_Category FOREIGN KEY ([CategorySlug]) REFERENCES [blog].[Category] ([Slug]) ON DELETE CASCADE,
    CONSTRAINT FK_PostCategoryEvent_Post FOREIGN KEY ([PostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE CASCADE ON UPDATE CASCADE,
);
