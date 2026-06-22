using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models;
using DaejeonConstruction.Web.Models.Enums;
using DaejeonConstruction.Web.Models.ViewModels.Admin;
using DaejeonConstruction.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 시공사례 관리 (등록/수정/삭제) - 어닝/데크 분류, 시공 전/후 이미지 다중 업로드 지원
    /// </summary>
    public class WorkCaseController : AdminControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileUploadService _fileUploadService;

        public WorkCaseController(ApplicationDbContext db, IFileUploadService fileUploadService)
        {
            _db = db;
            _fileUploadService = fileUploadService;
        }

        // GET: /admin/workcase
        public async Task<IActionResult> Index()
        {
            var items = await _db.WorkCases
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();
            return View(items);
        }

        // GET: /admin/workcase/create
        public IActionResult Create()
        {
            return View(new WorkCaseFormViewModel());
        }

        // POST: /admin/workcase/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkCaseFormViewModel form)
        {
            if (form.ThumbnailFile == null)
            {
                ModelState.AddModelError(nameof(form.ThumbnailFile), "썸네일 이미지를 등록해 주세요.");
            }

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var thumbPath = await _fileUploadService.SaveAsync(form.ThumbnailFile, "works");

            var entity = new WorkCase
            {
                Title = form.Title,
                Category = form.Category,
                Location = form.Location,
                Description = form.Description,
                ThumbnailPath = thumbPath!,
                SortOrder = form.SortOrder,
                IsPublished = form.IsPublished,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.WorkCases.Add(entity);
            await _db.SaveChangesAsync();

            await SaveWorkImagesAsync(entity.Id, form.BeforeImages, WorkImageType.Before);
            await SaveWorkImagesAsync(entity.Id, form.AfterImages, WorkImageType.After);

            TempData["Message"] = "시공사례가 등록되었습니다.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /admin/workcase/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.WorkCases
                .Include(w => w.Images)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null) return NotFound();

            var form = new WorkCaseFormViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Category = entity.Category,
                Location = entity.Location,
                Description = entity.Description,
                ExistingThumbnailPath = entity.ThumbnailPath,
                SortOrder = entity.SortOrder,
                IsPublished = entity.IsPublished,
                ExistingImages = entity.Images.OrderBy(i => i.SortOrder).ToList()
            };

            return View(form);
        }

        // POST: /admin/workcase/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkCaseFormViewModel form)
        {
            var entity = await _db.WorkCases
                .Include(w => w.Images)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
            {
                form.ExistingThumbnailPath = entity.ThumbnailPath;
                form.ExistingImages = entity.Images.OrderBy(i => i.SortOrder).ToList();
                return View(form);
            }

            entity.Title = form.Title;
            entity.Category = form.Category;
            entity.Location = form.Location;
            entity.Description = form.Description;
            entity.SortOrder = form.SortOrder;
            entity.IsPublished = form.IsPublished;
            entity.UpdatedAt = DateTime.Now;

            if (form.ThumbnailFile != null)
            {
                var newPath = await _fileUploadService.SaveAsync(form.ThumbnailFile, "works");
                if (newPath != null)
                {
                    _fileUploadService.Delete(entity.ThumbnailPath);
                    entity.ThumbnailPath = newPath;
                }
            }

            await _db.SaveChangesAsync();

            await SaveWorkImagesAsync(entity.Id, form.BeforeImages, WorkImageType.Before);
            await SaveWorkImagesAsync(entity.Id, form.AfterImages, WorkImageType.After);

            TempData["Message"] = "시공사례가 수정되었습니다.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /admin/workcase/delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.WorkCases
                .Include(w => w.Images)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null) return NotFound();

            _fileUploadService.Delete(entity.ThumbnailPath);
            foreach (var img in entity.Images)
            {
                _fileUploadService.Delete(img.ImagePath);
            }

            _db.WorkCases.Remove(entity); // WorkImage 는 Cascade 삭제
            await _db.SaveChangesAsync();

            TempData["Message"] = "시공사례가 삭제되었습니다.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /admin/workcase/deleteimage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int imageId, int workCaseId)
        {
            var image = await _db.WorkImages.FindAsync(imageId);
            if (image != null)
            {
                _fileUploadService.Delete(image.ImagePath);
                _db.WorkImages.Remove(image);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id = workCaseId });
        }

        private async Task SaveWorkImagesAsync(int workCaseId, List<IFormFile>? files, WorkImageType type)
        {
            if (files == null || files.Count == 0) return;

            int sortOrder = 0;
            foreach (var file in files.Where(f => f.Length > 0))
            {
                var path = await _fileUploadService.SaveAsync(file, "works");
                if (path != null)
                {
                    _db.WorkImages.Add(new WorkImage
                    {
                        WorkCaseId = workCaseId,
                        ImagePath = path,
                        ImageType = type,
                        SortOrder = sortOrder++,
                        CreatedAt = DateTime.Now
                    });
                }
            }
            await _db.SaveChangesAsync();
        }
    }
}
