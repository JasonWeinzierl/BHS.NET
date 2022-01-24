CREATE PROCEDURE [banners].[SiteBanner_InsertInitial]
	@date DATETIMEOFFSET,
	@theme TINYINT = 0,
	@lead NVARCHAR(4000) = NULL,
	@body NVARCHAR(4000) = NULL,
	@isEnabled BIT = 1
AS
BEGIN
	DECLARE @_inNestedTransaction BIT;

	BEGIN TRY
	
		IF	@@TRANCOUNT = 0
		BEGIN
			SET	@_inNestedTransaction = 0;
			BEGIN TRANSACTION;
		END
		ELSE
		BEGIN
			SET @_inNestedTransaction = 1;
		END;

		
		INSERT INTO
				[banners].[SiteBanner]
				([ThemeId], [Lead], [Body])
		VALUES	(@theme, @lead, @body);

		DECLARE @_bannerId INT = SCOPE_IDENTITY();
		EXEC [banners].[SiteBanner_SetEnabled] @_bannerId, @isEnabled, @date;


		IF	@@TRANCOUNT > 0
			AND @_inNestedTransaction = 0
		BEGIN
			COMMIT TRANSACTION;
		END;

	END TRY
	BEGIN CATCH
		
		IF	@@TRANCOUNT > 0
			AND @_inNestedTransaction = 0
		BEGIN
			ROLLBACK TRANSACTION;
		END;

		THROW;

	END CATCH;
END;
