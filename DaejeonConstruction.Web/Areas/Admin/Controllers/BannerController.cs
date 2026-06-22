using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models;
using DaejeonConstruction.Web.Models.ViewModels.Admin;
using DaejeonConstruction.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 메인 롤링배너 관리 (등록/수정/삭제)
    /// </summary>
    public class BannerController : AdminControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IFileUploadService _fileUploadService;

        public BannerController(ApplicationDbContext db, IFileUploadService fileUploadService)
        {
            _db = db;
            _fileUploadService = fileUploadService;
        }

        // GET: /admin/banner
        public async Task<IActionResult> Index()
        {
            var banners = await _db.MainBanners
                .OrderBy(b => b.SortOrder)
                .ToListAsync();
            return View(banners);
        }

        // GET: /admin/banner/create
        public IActionResult Create()
        {
            return View(new BannerFormViewModel());
        }

        // POST: /admin/banner/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerFormViewModel form)
        {
            if (form.ImageFile == null)
            {
                ModelState.AddModelError(nameof(form.ImageFile), "배너 이미지를 등록해 주세요.");
            }

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var imagePath = await _fileUploadService.SaveAsync(form.ImageFile, "banners");

            var entity = new MainBanner
            {
                Eyebrow = form.Eyebrow,
                Title = form.Title,
                SubText = form.SubText,
                ButtonText = form.ButtonText,
                ButtonLink = form.ButtonLink,
                ImagePath = imagePath!,
                SortOrder = form.SortOrder,
                IsActive = form.IsActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.MainBanners.Add(entity);
            await _db.SaveChangesAsync();

            TempData["Message"] = "배너가 등록되었습니다.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /admin/banner/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.MainBanners.FindAsync(id);
            if (entity == null) return NotFound();

            var form = new BannerFormViewModel
            {
                Id = entity.Id,
                Eyebrow = entity.Eyebrow,
                Title = entity.Title,
                SubText = entity.SubText,
                ButtonText = entity.ButtonText,
                ButtonLink = entity.ButtonLink,
                ExistingImagePath = entity.ImagePath,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive
            };

            return View(form);
        }

        // POST: /admin/banner/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BannerFormViewModel form)
        {
            var entity = await _db.MainBanners.FindAsync(id);
            if (entity == null) return NotFound();

            if (!ModelState.IsValid)
            {
                form.ExistingImagePath = entity.ImagePath;
                return View(form);
            }

            entity.Eyebrow = form.Eyebrow;
            entity.Title = form.Title;
            entity.SubText = form.SubText;
            entity.ButtonText = form.ButtonText;
            entity.ButtonLink = form.ButtonLink;
            entity.SortOrder = form.SortOrder;
            entity.IsActive = form.IsActive;
            entity.UpdatedAt = DateTime.Now;

            if (form.ImageFile != null)
            {
                var newPath = await _fileUploadService.SaveAsync(form.ImageFile, "banners");
                if (newPath != null)
                {
                    _fileUploadService.Delete(entity.ImagePath);
                    entity.ImagePath = newPath;
                }
            }

            await _db.SaveChangesAsync();

            TempData["Message"] = "배너가 수정되었습니다.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /admin/banner/delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.MainBanners.FindAsync(id);
            if (entity == null) return NotFound();

            _fileUploadService.Delete(entity.ImagePath);
            _db.MainBanners.Remove(entity);
            await _db.SaveChangesAsync();

            TempData["Message"] = "배너가 삭제되었습니다.";
            return RedirectToAction(nameof(Index));
        }
    }
}
