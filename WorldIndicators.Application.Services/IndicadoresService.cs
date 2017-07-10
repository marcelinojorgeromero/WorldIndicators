using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldIndicators.Persistence.Ado;
using Dto = WorldIndicators.Application.Dto;

namespace WorldIndicators.Application.Services
{
    public class IndicadoresService
    {
        private readonly DbContextBase _dbContext;

        public IndicadoresService()
        {
            _dbContext = new DbContextBase();
        }

        public List<Dto.HomeMainDataTable> GetDataTableMain(Dto.DtParameters parameter, out int totalRow, out int totalFilter)
        {
            Func<string, DateTime?> parseDate = datestr =>
            {
                DateTime parsedDate;
                return DateTime.TryParse(datestr, out parsedDate) ? parsedDate : (DateTime?) null;
            };

            var displayStart = parameter.iDisplayStart;
            var displayLength = parameter.iDisplayLength;

            var countryCode = string.IsNullOrEmpty(parameter.sSearch_a) ? null : string.Empty;
            var startDate = parseDate(parameter.sSearch_b);
            var endDate = parseDate(parameter.sSearch_c);
            var total = new SqlParameter("@total", SqlDbType.Int) {Direction = ParameterDirection.Output};
            
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("offset", displayStart),
                new SqlParameter("fetch", displayLength),
                new SqlParameter("country_code", countryCode),
                new SqlParameter("start_date", startDate?.Year),
                new SqlParameter("end_date", endDate?.Year),
                total
            };

            var list = new List<Dto.HomeMainDataTable>();
            _dbContext.Query("sp_c_datatable_indicators_natalidad_mortalidad", spParameters, reader =>
            {
                var mraf = reader["MortalityRateAdultFemale"].ToString();
                var mrafD = decimal.Parse(mraf);

                list.Add(new Dto.HomeMainDataTable
                {
                    CountryName = reader["CountryName"].ToString(),
                    Year = int.Parse(reader["Year"].ToString()),
                    BirthRateCrude = decimal.Parse(reader["BirthRateCrude"].ToString()),
                    MortalityRateAdultMale = decimal.Parse(reader["MortalityRateAdultMale"].ToString()),
                    MortalityRateAdultFemale = decimal.Parse(reader["MortalityRateAdultFemale"].ToString()),
                    MortalityRateInfant = decimal.Parse(reader["MortalityRateAdultFemale"].ToString()),
                    MortalityRateUnder5 = decimal.Parse(reader["MortalityRateUnder5"].ToString()),
                    MortalityRate = decimal.Parse(reader["MortalityRate"].ToString())
                });
            });

            totalFilter = totalRow = Convert.ToInt32(total.Value);

            return list;
        }

        public List<Dto.Country> GetCountriesDropDown()
        {
            var countries = new List<Dto.Country>();
            _dbContext.Query("sp_c_countries_drop_down", reader =>
            {
                countries.Add(new Dto.Country
                {
                    Code = reader["CountryCode"].ToString(),
                    ShortName = reader["ShortName"].ToString()
                });
            });
            return countries;
        } 
    }
}
