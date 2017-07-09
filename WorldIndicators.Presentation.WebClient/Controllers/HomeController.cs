using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dto = WorldIndicators.Application.Dto;
using WorldIndicators.Application.Services;
using Infrastructure.Utils;

namespace WorldIndicators.Presentation.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly Lazy<IndicadoresService> _indicadoresService;
         
        public HomeController()
        {
            _indicadoresService = new Lazy<IndicadoresService>(() => new IndicadoresService());
        }

        public ActionResult Index()
        {
            var model = new Dto.HomeIndex
            {
                Countries = _indicadoresService.Value.GetCountriesDropDown()
            };
            return View(model);
        }

        [HttpGet]
        public string DataTableMain(Dto.DtParameters parameters)
        {
            int totalRow;
            int totalFilter;
            var dataTableResult = _indicadoresService.Value.GetDataTableMain(parameters, out totalRow, out totalFilter);
            return DataTable<List<Dto.HomeMainDataTable>>.SerializeToJson(parameters.sEcho, totalRow, totalFilter, dataTableResult);
        }

        // TODO: VALIDAR EL FORMULARIO CON DECORADORES
        [HttpPost]
        public void Filter()
        {
        }
    }
}