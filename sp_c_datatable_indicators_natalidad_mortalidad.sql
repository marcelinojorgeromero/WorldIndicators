
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Marcelino Jorge Romero
-- Create date: 10-07-2017
-- Description:	Trae la grilla con los indicadores de natalidad y tasa de mortalidad de la página principal
-- =============================================
CREATE PROCEDURE sp_c_datatable_indicators_natalidad_mortalidad
	@offset INT = 0,
	@fetch INT = 10,
	@country_code VARCHAR(3) = NULL,
	@start_date INT = NULL,
	@end_date INT = NULL,
	@total INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	--DECLARE
	--	@offset INT = 0,
	--	@fetch INT = 10,
	--	@country_code VARCHAR(3) = NULL,
	--	@start_date INT = NULL,
	--	@end_date INT = NULL;

	--DECLARE
	--	@total INT;

	--SET @country_code = 'CHN';
	--SET @start_date = 1970;
	--SET @end_date = 1973;

	WITH IndicatorsBasic AS
	(
		SELECT DISTINCT
			CountryName,
			CountryCode,
			Year
		FROM dbo.Indicators
	)
	SELECT
		IndicatorsBasic.CountryName,
		IndicatorsBasic.CountryCode,
		IndicatorsBasic.Year,
		BirthRateCrudeTb.Value AS BirthRateCrude,
		MortalityRateAdultMaleTb.Value AS MortalityRateAdultMale,
		MortalityRateAdultFemaleTb.Value AS MortalityRateAdultFemale,
		MortalityRateInfantTb.Value AS MortalityRateInfant,
		MortalityRateUnder5Tb.Value AS MortalityRateUnder5,
		(MortalityRateAdultMaleTb.Value + MortalityRateAdultFemaleTb.Value + MortalityRateInfantTb.Value + MortalityRateUnder5Tb.Value) AS MortalityRate
	INTO #IndicatorsComplete
	FROM IndicatorsBasic
	JOIN dbo.Indicators AS BirthRateCrudeTb
		ON BirthRateCrudeTb.CountryCode = IndicatorsBasic.CountryCode
		AND BirthRateCrudeTb.Year = IndicatorsBasic.Year
	JOIN dbo.Indicators AS MortalityRateAdultMaleTb
		ON MortalityRateAdultMaleTb.CountryCode = BirthRateCrudeTb.CountryCode
		AND MortalityRateAdultMaleTb.Year = IndicatorsBasic.Year
	JOIN dbo.Indicators AS MortalityRateAdultFemaleTb
		ON MortalityRateAdultFemaleTb.CountryCode = IndicatorsBasic.CountryCode
		AND MortalityRateAdultFemaleTb.Year = IndicatorsBasic.Year
	JOIN dbo.Indicators AS MortalityRateInfantTb
		ON MortalityRateInfantTb.CountryCode = IndicatorsBasic.CountryCode
		AND MortalityRateInfantTb.Year = IndicatorsBasic.Year
	JOIN dbo.Indicators AS MortalityRateUnder5Tb
		ON MortalityRateUnder5Tb.CountryCode = IndicatorsBasic.CountryCode
		AND MortalityRateUnder5Tb.Year = IndicatorsBasic.Year
	WHERE
		BirthRateCrudeTb.IndicatorCode = 'SP.DYN.CBRT.IN'
		AND MortalityRateAdultMaleTb.IndicatorCode = 'SP.DYN.AMRT.MA'
		AND MortalityRateAdultFemaleTb.IndicatorCode = 'SP.DYN.AMRT.FE'
		AND MortalityRateInfantTb.IndicatorCode = 'SP.DYN.IMRT.IN'
		AND MortalityRateUnder5Tb.IndicatorCode = 'SH.DYN.MORT'

	SELECT @total = COUNT(*) FROM #IndicatorsComplete;

	SELECT
		CountryName,
		Year,
		BirthRateCrude,
		MortalityRateAdultMale,
		MortalityRateAdultFemale,
		MortalityRateInfant,
		MortalityRateUnder5,
		MortalityRate
	FROM #IndicatorsComplete
	WHERE
		(@country_code IS NULL OR CountryCode = @country_code)
		AND (
			(@start_date IS NULL AND @end_date IS NULL) OR
			(@start_date IS NOT NULL AND @end_date IS NOT NULL AND Year BETWEEN @start_date AND @end_date) OR
			(@start_date IS NULL AND @end_date IS NOT NULL AND Year <= @end_date) OR
			(@start_date IS NOT NULL AND @end_date IS NULL AND Year >= @start_date)
		)
	ORDER BY Year, CountryCode OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY

	DROP TABLE #IndicatorsComplete
END
GO
