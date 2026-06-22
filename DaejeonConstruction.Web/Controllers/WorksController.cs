using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models.Enums;
using DaejeonConstruction.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Controllers
{
    /// <summary>
    /// 시공사례 목록/상세보기 (일반 사용자)
    /// </summary>
    public class WorksController : Controller
    {
        private readonly ApplicationDbContext _db;

        public WorksController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Works?category=Awning
        public async Task<IActionResult> Index(WorkCategory? category)
        {
            var query = _db.WorkCases.Where(w => w.IsPublished);

            if (category.HasValue)
            {
                query = query.Where(w => w.Category == category.Value);
            }

            var items = await query
                .OrderByDescending(w => w.SortOrder)
                .ThenByDescending(w => w.CreatedAt)
                .ToListAsync();

            var vm = new WorksIndexViewModel
            {
                Items = items,
                FilterCategory = category
            };

            return View(vm);
        }

        // GET: /Works/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.WorkCases
                .Include(w => w.Images)
                .FirstOrDefaultAsync(w => w.Id == id && w.IsPublished);

            if (item == null) return NotFound();

            return View(item);
        }
    }
}
