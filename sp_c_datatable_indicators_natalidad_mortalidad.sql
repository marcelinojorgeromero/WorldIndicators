
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

	SELECT
		Indicators.CountryName, Indicators.CountryCode, Indicators.IndicatorCode, Indicators.Year, Indicators.Value
	INTO #IndicatorsPartial
	FROM dbo.Indicators AS Indicators
	WHERE Indicators.IndicatorCode IN ('SP.DYN.CBRT.IN', 'SP.DYN.AMRT.MA', 'SP.DYN.AMRT.FE', 'SP.DYN.IMRT.IN', 'SH.DYN.MORT')


	;WITH IndicatorsCountries AS
	(
		SELECT DISTINCT
			CountryName,
			CountryCode,
			Year
		FROM #IndicatorsPartial
	)
	SELECT
		IndicatorsCountries.CountryName,
		IndicatorsCountries.CountryCode,
		IndicatorsCountries.Year,
		BirthRateCrudeTb.Value AS BirthRateCrude,
		MortalityRateAdultMaleTb.Value AS MortalityRateAdultMale,
		MortalityRateAdultFemaleTb.Value AS MortalityRateAdultFemale,
		MortalityRateInfantTb.Value AS MortalityRateInfant,
		MortalityRateUnder5Tb.Value AS MortalityRateUnder5,
		(MortalityRateAdultMaleTb.Value + MortalityRateAdultFemaleTb.Value + MortalityRateInfantTb.Value + MortalityRateUnder5Tb.Value) AS MortalityRate
	INTO #IndicatorsComplete
	FROM IndicatorsCountries
	JOIN #IndicatorsPartial AS BirthRateCrudeTb
		ON BirthRateCrudeTb.CountryCode = IndicatorsCountries.CountryCode
		AND BirthRateCrudeTb.Year = IndicatorsCountries.Year
	JOIN #IndicatorsPartial AS MortalityRateAdultMaleTb
		ON MortalityRateAdultMaleTb.CountryCode = BirthRateCrudeTb.CountryCode
		AND MortalityRateAdultMaleTb.Year = IndicatorsCountries.Year
	JOIN #IndicatorsPartial AS MortalityRateAdultFemaleTb
		ON MortalityRateAdultFemaleTb.CountryCode = IndicatorsCountries.CountryCode
		AND MortalityRateAdultFemaleTb.Year = IndicatorsCountries.Year
	JOIN #IndicatorsPartial AS MortalityRateInfantTb
		ON MortalityRateInfantTb.CountryCode = IndicatorsCountries.CountryCode
		AND MortalityRateInfantTb.Year = IndicatorsCountries.Year
	JOIN #IndicatorsPartial AS MortalityRateUnder5Tb
		ON MortalityRateUnder5Tb.CountryCode = IndicatorsCountries.CountryCode
		AND MortalityRateUnder5Tb.Year = IndicatorsCountries.Year
	WHERE
		BirthRateCrudeTb.IndicatorCode = 'SP.DYN.CBRT.IN'
		AND MortalityRateAdultMaleTb.IndicatorCode = 'SP.DYN.AMRT.MA'
		AND MortalityRateAdultFemaleTb.IndicatorCode = 'SP.DYN.AMRT.FE'
		AND MortalityRateInfantTb.IndicatorCode = 'SP.DYN.IMRT.IN'
		AND MortalityRateUnder5Tb.IndicatorCode = 'SH.DYN.MORT'
		AND (@country_code IS NULL OR IndicatorsCountries.CountryCode = @country_code)
		AND (
			(@start_date IS NULL AND @end_date IS NULL) OR
			(@start_date IS NOT NULL AND @end_date IS NOT NULL AND IndicatorsCountries.Year BETWEEN @start_date AND @end_date) OR
			(@start_date IS NULL AND @end_date IS NOT NULL AND IndicatorsCountries.Year <= @end_date) OR
			(@start_date IS NOT NULL AND @end_date IS NULL AND IndicatorsCountries.Year >= @start_date)
		)
	
	DROP TABLE #IndicatorsPartial

	SELECT @total = COUNT(*) FROM #IndicatorsComplete;

	SELECT
		CountryName,
		CountryCode,
		Year,
		BirthRateCrude,
		MortalityRateAdultMale,
		MortalityRateAdultFemale,
		MortalityRateInfant,
		MortalityRateUnder5,
		MortalityRate
	FROM #IndicatorsComplete
	ORDER BY CountryCode, Year OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY

	DROP TABLE #IndicatorsComplete
END
GO
