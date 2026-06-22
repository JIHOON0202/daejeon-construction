using System.ComponentModel.DataAnnotations;

namespace DaejeonConstruction.Web.Models
{
    /// <summary>
    /// 메인 롤링배너 (MAIN_BANNER)
    /// </summary>
    public class MainBanner
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Eyebrow { get; set; } = string.Empty; // 작은 상단 문구 (예: 대전시공사)

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty; // 큰 제목 (예: 어닝 · 데크 전문 시공)

        [MaxLength(200)]
        public string? SubText { get; set; } // 강조 부제 (예: 무료 견적 상담 가능)

        [MaxLength(50)]
        public string ButtonText { get; set; } = "견적문의 바로가기 →";

        [MaxLength(300)]
        public string ButtonLink { get; set; } = "#quote";

        [Required, MaxLength(300)]
        public string ImagePath { get; set; } = string.Empty; // wwwroot 기준 상대경로

        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
