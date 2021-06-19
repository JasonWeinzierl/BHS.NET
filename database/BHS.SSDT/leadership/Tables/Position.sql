CREATE TABLE [leadership].[Position]
(
	[Id] INT IDENTITY(1,1) CONSTRAINT Position_PK PRIMARY KEY,

	[Title] NVARCHAR(255) NOT NULL,
	[SortOrder] INT NOT NULL,
);
