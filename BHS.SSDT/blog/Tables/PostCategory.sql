CREATE TABLE [blog].[PostCategory] (
    [CategoryId] INT NOT NULL,
    [PostId]     INT NOT NULL,
    CONSTRAINT PK_BlogPostCategory PRIMARY KEY (CategoryId, PostId),
    CONSTRAINT FK_BlogPostCategory_Category_CategoryId FOREIGN KEY (CategoryId) REFERENCES [blog].[Category] ([Id]) ON DELETE CASCADE,
    CONSTRAINT FK_BlogPostCategory_Post_PostId FOREIGN KEY (PostId) REFERENCES [blog].[Post] ([Id]) ON DELETE CASCADE
);

