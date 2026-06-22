using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models.Enums;
using DaejeonConstruction.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeIndexViewModel
            {
                Banners = await _db.MainBanners
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.SortOrder)
                    .ToListAsync(),

                RecentWorks = await _db.WorkCases
                    .Where(w => w.IsPublished)
                    .OrderByDescending(w => w.CreatedAt)
                    .Take(6)
                    .ToListAsync()
            };

            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
