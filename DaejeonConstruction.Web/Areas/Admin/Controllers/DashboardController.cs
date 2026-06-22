using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.BannerCount = await _db.MainBanners.CountAsync();
            ViewBag.WorkCaseCount = await _db.WorkCases.CountAsync();
            ViewBag.EstimateTotalCount = await _db.EstimateRequests.CountAsync();
            ViewBag.EstimateReceivedCount = await _db.EstimateRequests.CountAsync(e => e.Status == EstimateStatus.Received);
            ViewBag.EstimateInProgressCount = await _db.EstimateRequests.CountAsync(e => e.Status == EstimateStatus.InProgress);
            ViewBag.EstimateCompletedCount = await _db.EstimateRequests.CountAsync(e => e.Status == EstimateStatus.Completed);

            ViewBag.RecentEstimates = await _db.EstimateRequests
                .OrderByDescending(e => e.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
