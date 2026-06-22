using DaejeonConstruction.Web.Models;
using DaejeonConstruction.Web.Models.Enums;
using DaejeonConstruction.Web.Services;

namespace DaejeonConstruction.Web.Data
{
    /// <summary>
    /// 앱 시작 시 마이그레이션 적용 + 초기 데이터(관리자 계정, 샘플 배너/시공사례) 시드
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            db.Database.Migrate();

            // 1) 기본 관리자 계정 (admin / admin1234) - 최초 1회만 생성
            if (!db.AdminUsers.Any())
            {
                db.AdminUsers.Add(new AdminUser
                {
                    Username = "admin",
                    PasswordHash = hasher.HashPassword("admin1234"),
                    DisplayName = "관리자",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                });
            }

            // 2) 샘플 배너 (운영 시 관리자 페이지에서 실제 이미지로 교체)
            if (!db.MainBanners.Any())
            {
                db.MainBanners.AddRange(
                    new MainBanner
                    {
                        Eyebrow = "대전시공사",
                        Title = "어닝 · 데크 전문 시공",
                        SubText = "무료 견적 상담 가능",
                        ButtonText = "견적문의 바로가기 →",
                        ButtonLink = "#quote",
                        ImagePath = "/uploads/banners/sample-bg1.jpg",
                        SortOrder = 1,
                        IsActive = true
                    },
                    new MainBanner
                    {
                        Eyebrow = "꼼꼼한 현장 시공",
                        Title = "상가 · 카페 · 주택 맞춤 시공",
                        SubText = "합리적인 가격과 확실한 마감",
                        ButtonText = "전화 상담하기 →",
                        ButtonLink = "tel:042-222-2222",
                        ImagePath = "/uploads/banners/sample-bg2.jpg",
                        SortOrder = 2,
                        IsActive = true
                    },
                    new MainBanner
                    {
                        Eyebrow = "어닝 & 데크",
                        Title = "공간에 어울리는 시공 제안",
                        SubText = "상담부터 시공까지 한 번에",
                        ButtonText = "카카오톡 상담 →",
                        ButtonLink = "https://open.kakao.com/o/sAa6exwi",
                        ImagePath = "/uploads/banners/sample-bg3.jpg",
                        SortOrder = 3,
                        IsActive = true
                    }
                );
            }

            db.SaveChanges();
        }
    }
}
