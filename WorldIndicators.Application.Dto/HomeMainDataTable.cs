
namespace WorldIndicators.Application.Dto
{
    public class HomeMainDataTable
    {
        public string CountryName { get; set; }
        public int Year { get; set; }
        public decimal BirthRateCrude { get; set; }
        public decimal MortalityRateAdultMale { get; set; }
        public decimal MortalityRateAdultFemale { get; set; }
        public decimal MortalityRateInfant { get; set; }
        public decimal MortalityRateUnder5 { get; set; }
        public decimal MortalityRate { get; set; }
    }
}
