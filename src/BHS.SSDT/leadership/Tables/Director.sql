﻿CREATE TABLE [leadership].[Director]
(
	[Id] INT IDENTITY(1,1) CONSTRAINT Director_PK PRIMARY KEY,

	[Name] NVARCHAR(MAX) NOT NULL,
	[Year] INT NOT NULL,
);
