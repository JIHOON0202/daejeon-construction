using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DaejeonConstruction.Web.Models.Enums;

namespace DaejeonConstruction.Web.Models
{
    /// <summary>
    /// 시공사례 상세 이미지 (WORK_IMAGE) - 시공 전/후 등
    /// </summary>
    public class WorkImage
    {
        public int Id { get; set; }

        [Required]
        public int WorkCaseId { get; set; }

        [ForeignKey(nameof(WorkCaseId))]
        public WorkCase? WorkCase { get; set; }

        [Required, MaxLength(300)]
        public string ImagePath { get; set; } = string.Empty;

        public WorkImageType ImageType { get; set; } = WorkImageType.Gallery;

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
