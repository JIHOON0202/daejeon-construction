using DaejeonConstruction.Web.Models;

namespace DaejeonConstruction.Web.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<MainBanner> Banners { get; set; } = new();
        public List<WorkCase> AwningCases { get; set; } = new();
        public List<WorkCase> DeckCases { get; set; } = new();
        public List<WorkCase> RecentWorks { get; set; } = new();
        public EstimateCreateViewModel Estimate { get; set; } = new();
    }
}
