using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 견적문의 관리 - 목록 / 상세조회 / 상태변경(접수,상담중,완료)
    /// </summary>
    public class EstimateController : AdminControllerBase
    {
        private readonly ApplicationDbContext _db;

        public EstimateController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /admin/estimate?status=Received
        public async Task<IActionResult> Index(EstimateStatus? status)
        {
            var query = _db.EstimateRequests.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            var items = await query
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            ViewBag.FilterStatus = status;
            return View(items);
        }

        // GET: /admin/estimate/details/5
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.EstimateRequests
                .Include(e => e.Files)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: /admin/estimate/changestatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, EstimateStatus status, string? adminMemo)
        {
            var item = await _db.EstimateRequests.FindAsync(id);
            if (item == null) return NotFound();

            item.Status = status;
            item.AdminMemo = adminMemo;
            item.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();

            TempData["Message"] = "상태가 변경되었습니다.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /admin/estimate/delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.EstimateRequests.FindAsync(id);
            if (item == null) return NotFound();

            _db.EstimateRequests.Remove(item);
            await _db.SaveChangesAsync();

            TempData["Message"] = "문의가 삭제되었습니다.";
            return RedirectToAction(nameof(Index));
        }
    }
}
