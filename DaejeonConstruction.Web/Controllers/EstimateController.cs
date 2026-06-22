using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models;
using DaejeonConstruction.Web.Models.ViewModels;
using DaejeonConstruction.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace DaejeonConstruction.Web.Controllers
{
    /// <summary>
    /// 견적문의 등록 (일반 사용자) - 메인 페이지의 견적문의 폼에서 호출됨
    /// </summary>
    public class EstimateController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileUploadService _fileUploadService;

        public EstimateController(ApplicationDbContext db, IFileUploadService fileUploadService)
        {
            _db = db;
            _fileUploadService = fileUploadService;
        }

        // POST: /Estimate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstimateCreateViewModel form)
        {
            if (!ModelState.IsValid)
            {
                TempData["EstimateError"] = "성함과 연락처는 필수 입력 항목입니다.";
                return RedirectToAction("Index", "Home", new { }, "quote");
            }

            var entity = new EstimateRequest
            {
                Name = form.Name.Trim(),
                Phone = form.Phone.Trim(),
                Area = form.Area?.Trim(),
                ServiceType = form.ServiceType,
                Message = form.Message?.Trim(),
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.EstimateRequests.Add(entity);
            await _db.SaveChangesAsync();

            if (form.Photos != null && form.Photos.Count > 0)
            {
                foreach (var photo in form.Photos.Where(p => p.Length > 0))
                {
                    var path = await _fileUploadService.SaveAsync(photo, "estimates");
                    if (path != null)
                    {
                        _db.EstimateFiles.Add(new EstimateFile
                        {
                            EstimateRequestId = entity.Id,
                            FilePath = path,
                            FileName = photo.FileName,
                            FileSize = photo.Length,
                            UploadedAt = DateTime.Now
                        });
                    }
                }
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Complete), new { id = entity.Id });
        }

        // GET: /Estimate/Complete/5
        public async Task<IActionResult> Complete(int id)
        {
            var entity = await _db.EstimateRequests.FindAsync(id);
            if (entity == null) return NotFound();

            return View(entity);
        }
    }
}
