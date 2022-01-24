CREATE PROCEDURE [banners].[SiteBanner_SetEnabled]
	@id INT,
	@isEnabled BIT,
	@date DATETIMEOFFSET
AS
BEGIN
	INSERT INTO
			[banners].[SiteBannerEvent]
			([SiteBannerId], [EventDate], [IsEnabled])
	VALUES	(@id, @date, @isEnabled);
END;
