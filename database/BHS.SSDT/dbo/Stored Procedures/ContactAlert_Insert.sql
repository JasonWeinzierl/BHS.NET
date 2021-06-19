CREATE PROCEDURE [dbo].[ContactAlert_Insert]
	@name NVARCHAR(255),
	@emailAddress NVARCHAR(255),
	@message NVARCHAR(MAX),
	@dateRequested DATETIMEOFFSET
AS
BEGIN
	INSERT INTO
			[dbo].[ContactAlert] ([Name], [EmailAddress], [Message], [DateRequested])
	OUTPUT	INSERTED.[Id]
			, INSERTED.[Name]
			, INSERTED.[EmailAddress]
			, INSERTED.[Message]
			, INSERTED.[DateRequested]
			, INSERTED.[DateCreated]
	VALUES	(@name, @emailAddress, @message, @dateRequested);
END
