CREATE TABLE [banners].[SiteBannerEvent]
(
	[SiteBannerId] INT NOT NULL,
	[EventDate] DATETIMEOFFSET NOT NULL CONSTRAINT DF_SiteBannerEvent_EventDate DEFAULT SYSDATETIMEOFFSET(),

	[IsEnabled] BIT NOT NULL,

	CONSTRAINT PK_SiteBannerEvent PRIMARY KEY ([SiteBannerId], [EventDate]),
	CONSTRAINT FK_SiteBannerEvent_SiteBanner FOREIGN KEY ([SiteBannerId]) REFERENCES [banners].[SiteBanner] ([Id]) ON DELETE CASCADE,
);
