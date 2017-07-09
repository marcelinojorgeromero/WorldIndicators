using System;
using System.Collections.Generic;
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
            var displayStart = parameter.iDisplayStart;
            var displayLength = parameter.iDisplayLength;

            //var grupoDelUsuarioActual = parameter.sSearch_d.ToInt();

            var list = new List<Dto.HomeMainDataTable>();

            //var list = _repositoryMarca.GetWithPaging(item => item.IdGrupo == grupoDelUsuarioActual,
            //    item => item.Prioridad, EnumSortOrder.Asc, displayStart, displayLength, out totalRow)
            //    .ToList()
            //    .Select(item => new Dto.DataTableMainMarcas
            //    {
            //        IdMarca = item.IdMarca,
            //        TipoMarca = Convert.ToChar(item.TipoMarca),
            //        Prioridad = item.Prioridad,
            //        Vigente = item.Vigente
            //    })
            //    .ToList();

            totalFilter = totalRow = 0;

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
