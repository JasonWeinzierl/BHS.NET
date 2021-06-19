CREATE TABLE [blog].[Category] (
    [Slug] VARCHAR(127) NOT NULL CONSTRAINT PK_Category PRIMARY KEY,
    [Name] NVARCHAR (127) NOT NULL,
    CONSTRAINT CK_Category_Slug_Alphanumeric CHECK ([Slug] NOT LIKE '%[^A-Z0-9-]%')
);
