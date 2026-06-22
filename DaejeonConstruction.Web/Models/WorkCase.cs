using System.ComponentModel.DataAnnotations;
using DaejeonConstruction.Web.Models.Enums;

namespace DaejeonConstruction.Web.Models
{
    /// <summary>
    /// 시공사례 (WORK_CASE)
    /// </summary>
    public class WorkCase
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public WorkCategory Category { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; } // 시공지역

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required, MaxLength(300)]
        public string ThumbnailPath { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public bool IsPublished { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<WorkImage> Images { get; set; } = new List<WorkImage>();
    }
}
