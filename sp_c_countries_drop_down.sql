SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Marcelino Jorge Romero
-- Create date: 09-06-2017
-- Description:	Gets the countries for a drop down list
-- =============================================
CREATE PROCEDURE sp_c_countries_drop_down
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CountryCode, ShortName
	FROM dbo.Country 
	ORDER BY CountryCode ASC
END
GO
