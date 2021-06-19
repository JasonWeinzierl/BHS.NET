CREATE TABLE [blog].[Post] (
    [Slug] VARCHAR(127) NOT NULL CONSTRAINT PK_Post PRIMARY KEY,
    CONSTRAINT CK_Post_Slug_Alphanumeric CHECK ([Slug] NOT LIKE '%[^A-Z0-9-]%')
);
