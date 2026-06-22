using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Models.ViewModels.Admin;
using DaejeonConstruction.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaejeonConstruction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher _hasher;

        public AccountController(ApplicationDbContext db, IPasswordHasher hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        // GET: /admin/account/login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (HttpContext.Session.GetInt32(AdminControllerBase.SessionKeyAdminId) != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // POST: /admin/account/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var user = await _db.AdminUsers
                .FirstOrDefaultAsync(u => u.Username == form.Username && u.IsActive);

            if (user == null || !_hasher.VerifyPassword(form.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "아이디 또는 비밀번호가 올바르지 않습니다.");
                return View(form);
            }

            HttpContext.Session.SetInt32(AdminControllerBase.SessionKeyAdminId, user.Id);
            HttpContext.Session.SetString(AdminControllerBase.SessionKeyAdminName, user.DisplayName ?? user.Username);

            user.LastLoginAt = DateTime.Now;
            await _db.SaveChangesAsync();

            if (!string.IsNullOrEmpty(form.ReturnUrl) && Url.IsLocalUrl(form.ReturnUrl))
            {
                return Redirect(form.ReturnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        // POST: /admin/account/logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
