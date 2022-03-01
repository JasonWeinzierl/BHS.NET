CREATE TABLE [leadership].[OfficerPositionStart]
(
	[Id] INT IDENTITY(1,1) CONSTRAINT OfficerPosition_PK PRIMARY KEY NONCLUSTERED,
	[OfficerId] INT NULL,
	[PositionId] INT NOT NULL,

	[DateStarted] DATETIMEOFFSET NOT NULL,
	
	INDEX UK_OfficerPosition_DateStartedPositionId UNIQUE CLUSTERED ([DateStarted], [PositionId]),
    CONSTRAINT FK_OfficerPosition_Officer FOREIGN KEY ([OfficerId]) REFERENCES [leadership].[Officer] ([Id]) ON DELETE SET NULL,
    CONSTRAINT FK_OfficerPosition_Position FOREIGN KEY ([PositionId]) REFERENCES [leadership].[Position] ([Id]) ON DELETE CASCADE
);
