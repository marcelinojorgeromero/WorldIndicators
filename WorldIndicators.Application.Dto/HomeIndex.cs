using System.Collections.Generic;
using System.Data;

namespace WorldIndicators.Application.Dto
{
    public class HomeIndex
    {
        public string CountryCode { get; set; }
        public List<Country> Countries { get; set; }

        public HomeIndex()
        {
            Countries = new List<Country>();
        }
    }
}
