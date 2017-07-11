
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
		CountryName, CountryCode, IndicatorCode, Year, Value
	INTO #IndicatorsRaw
	FROM dbo.Indicators
	WHERE
		IndicatorCode IN ('SP.DYN.CBRT.IN', 'SP.DYN.AMRT.MA', 'SP.DYN.AMRT.FE', 'SP.DYN.IMRT.IN', 'SH.DYN.MORT')
		AND (@country_code IS NULL OR CountryCode = @country_code)
		AND (
			(@start_date IS NULL AND @end_date IS NULL) OR
			(@start_date IS NOT NULL AND @end_date IS NOT NULL AND Year BETWEEN @start_date AND @end_date) OR
			(@start_date IS NULL AND @end_date IS NOT NULL AND Year <= @end_date) OR
			(@start_date IS NOT NULL AND @end_date IS NULL AND Year >= @start_date)
		)

	-- Transformación de la tabla vertical anterior a horizontal
	;WITH IndicatorCountries AS
	(
		SELECT DISTINCT
			CountryName, CountryCode, Year
		FROM #IndicatorsRaw
	)
	SELECT
		IndicatorCountries.CountryName,
		IndicatorCountries.CountryCode,
		IndicatorCountries.Year,
		BirthRateCrudeTb.BirthRateCrude,
		MortalityRateAdultMaleTb.MortalityRateAdultMale,
		MortalityRateAdultFemaleTb.MortalityRateAdultFemale,
		MortalityRateInfantTb.MortalityRateInfant,
		MortalityRateUnder5Tb.MortalityRateUnder5,
		(MortalityRateAdultMaleTb.MortalityRateAdultMale + MortalityRateAdultFemaleTb.MortalityRateAdultFemale + 
		MortalityRateInfantTb.MortalityRateInfant + MortalityRateUnder5Tb.MortalityRateUnder5) AS MortalityRate
	INTO #IndicatorsComplete
	FROM IndicatorCountries
	CROSS APPLY
	(
		SELECT T.Value AS BirthRateCrude FROM #IndicatorsRaw AS T
		WHERE
			T.IndicatorCode = 'SP.DYN.CBRT.IN'
			AND T.CountryCode = IndicatorCountries.CountryCode
			AND T.Year = IndicatorCountries.Year
	) AS BirthRateCrudeTb
	CROSS APPLY
	(
		SELECT T.Value AS MortalityRateAdultMale FROM #IndicatorsRaw AS T
		WHERE
			T.IndicatorCode = 'SP.DYN.AMRT.MA'
			AND T.CountryCode = IndicatorCountries.CountryCode
			AND T.Year = IndicatorCountries.Year
	) AS MortalityRateAdultMaleTb
	CROSS APPLY
	(
		SELECT T.Value AS MortalityRateAdultFemale FROM #IndicatorsRaw AS T
		WHERE
			T.IndicatorCode = 'SP.DYN.AMRT.FE'
			AND T.CountryCode = IndicatorCountries.CountryCode
			AND T.Year = IndicatorCountries.Year

	) AS MortalityRateAdultFemaleTb
	CROSS APPLY
	(
		SELECT T.Value AS MortalityRateInfant FROM #IndicatorsRaw AS T
		WHERE
			T.IndicatorCode = 'SP.DYN.IMRT.IN'
			AND T.CountryCode = IndicatorCountries.CountryCode
			AND T.Year = IndicatorCountries.Year

	) AS MortalityRateInfantTb
	CROSS APPLY
	(
		SELECT T.Value AS MortalityRateUnder5 FROM #IndicatorsRaw AS T
		WHERE
			T.IndicatorCode = 'SH.DYN.MORT'
			AND T.CountryCode = IndicatorCountries.CountryCode
			AND T.Year = IndicatorCountries.Year

	) AS MortalityRateUnder5Tb

	DROP TABLE #IndicatorsRaw

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
