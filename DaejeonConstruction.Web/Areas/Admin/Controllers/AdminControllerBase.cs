using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DaejeonConstruction.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 관리자 영역 공통 베이스 컨트롤러.
    /// 세션에 로그인 정보가 없으면 로그인 페이지로 리다이렉트한다. (AccountController 는 제외)
    /// </summary>
    [Area("Admin")]
    public abstract class AdminControllerBase : Controller
    {
        public const string SessionKeyAdminId = "AdminUserId";
        public const string SessionKeyAdminName = "AdminDisplayName";

        protected int? CurrentAdminId => HttpContext.Session.GetInt32(SessionKeyAdminId);
        protected string? CurrentAdminName => HttpContext.Session.GetString(SessionKeyAdminName);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (CurrentAdminId == null)
            {
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "Admin", returnUrl });
                return;
            }

            ViewBag.CurrentAdminName = CurrentAdminName;
            base.OnActionExecuting(context);
        }
    }
}
