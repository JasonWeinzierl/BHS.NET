CREATE TABLE [blog].[PostCategoryEvent] (
    [CategorySlug] VARCHAR(127) NOT NULL,
    [PostSlug] VARCHAR(127) NOT NULL,
    [EventDate] DATETIMEOFFSET NOT NULL,
    [IsEnabled] BIT NOT NULL CONSTRAINT DF_PostCategory_IsEnabled DEFAULT ((1)),
    CONSTRAINT PK_PostCategoryEvent PRIMARY KEY ([CategorySlug], [PostSlug], [EventDate]),
    CONSTRAINT FK_PostCategoryEvent_Category_CategoryId FOREIGN KEY ([CategorySlug]) REFERENCES [blog].[Category] ([Slug]) ON DELETE CASCADE,
    CONSTRAINT FK_PostCategoryEvent_Post_PostId FOREIGN KEY ([PostSlug]) REFERENCES [blog].[Post] ([Slug]) ON DELETE CASCADE
);
