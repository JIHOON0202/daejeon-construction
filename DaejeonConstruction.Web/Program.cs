using DaejeonConstruction.Web.Data;
using DaejeonConstruction.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) MVC + Areas (관리자 영역 /admin 라우팅용)
builder.Services.AddControllersWithViews();

// 2) MySQL (Pomelo) + EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection 이 설정되지 않았습니다.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 3) 세션 (관리자 로그인 인증용)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.Name = "DaejeonConstruction.Admin.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4) 업로드 파일 크기 제한 (기본 30MB)
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 30 * 1024 * 1024;
});

// 5) 커스텀 서비스
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 앱 시작 시 마이그레이션 자동 적용 + 초기 데이터 시드
DbInitializer.Initialize(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// 관리자 영역 라우트 (예: /admin/banner/edit/3)
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

// 일반 사용자 라우트
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
