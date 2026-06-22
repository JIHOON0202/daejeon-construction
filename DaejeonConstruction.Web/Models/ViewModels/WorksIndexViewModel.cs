using DaejeonConstruction.Web.Models;
using DaejeonConstruction.Web.Models.Enums;

namespace DaejeonConstruction.Web.Models.ViewModels
{
    public class WorksIndexViewModel
    {
        public List<WorkCase> Items { get; set; } = new();
        public WorkCategory? FilterCategory { get; set; }
    }
}
