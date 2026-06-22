using System.ComponentModel.DataAnnotations;
using DaejeonConstruction.Web.Models.Enums;

namespace DaejeonConstruction.Web.Models
{
    /// <summary>
    /// 견적문의 (ESTIMATE_REQUEST)
    /// </summary>
    public class EstimateRequest
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty; // 성함

        [Required, MaxLength(30)]
        public string Phone { get; set; } = string.Empty; // 연락처

        [MaxLength(100)]
        public string? Area { get; set; } // 시공지역

        [MaxLength(50)]
        public string? ServiceType { get; set; } // 시공품목 (어닝/데크/어닝+데크)

        [MaxLength(2000)]
        public string? Message { get; set; } // 문의내용

        public EstimateStatus Status { get; set; } = EstimateStatus.Received;

        [MaxLength(2000)]
        public string? AdminMemo { get; set; } // 관리자 상담메모

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        public ICollection<EstimateFile> Files { get; set; } = new List<EstimateFile>();
    }
}
