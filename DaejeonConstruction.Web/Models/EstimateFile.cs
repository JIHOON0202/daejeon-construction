using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaejeonConstruction.Web.Models
{
    /// <summary>
    /// 견적문의 첨부파일 (ESTIMATE_FILE)
    /// </summary>
    public class EstimateFile
    {
        public int Id { get; set; }

        [Required]
        public int EstimateRequestId { get; set; }

        [ForeignKey(nameof(EstimateRequestId))]
        public EstimateRequest? EstimateRequest { get; set; }

        [Required, MaxLength(300)]
        public string FilePath { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
