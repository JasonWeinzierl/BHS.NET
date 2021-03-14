CREATE TABLE [dbo].[Log] (
    [Id] INT IDENTITY(1,1) CONSTRAINT PK_Log PRIMARY KEY,
    [Message] NVARCHAR(MAX) NULL,
    [MessageTemplate] NVARCHAR(MAX) NULL,
    [Level] TINYINT NOT NULL,
    [TimeStamp] DATETIMEOFFSET NOT NULL,
    [Exception] NVARCHAR(MAX) NULL,
    [LogEvent] NVARCHAR(MAX) NULL,
    [RequestPath] NVARCHAR(MAX) NULL,
);
