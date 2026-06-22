using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DaejeonConstruction.Web.Models.Enums;

namespace DaejeonConstruction.Web.Models.ViewModels.Admin
{
    public class WorkCaseFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "제목을 입력해 주세요.")]
        [Display(Name = "제목")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "분류")]
        public WorkCategory Category { get; set; }

        [Display(Name = "시공지역")]
        public string? Location { get; set; }

        [Display(Name = "설명")]
        public string? Description { get; set; }

        [Display(Name = "썸네일 이미지")]
        public IFormFile? ThumbnailFile { get; set; }

        public string? ExistingThumbnailPath { get; set; }

        [Display(Name = "시공 전 사진")]
        public List<IFormFile>? BeforeImages { get; set; }

        [Display(Name = "시공 후 사진")]
        public List<IFormFile>? AfterImages { get; set; }

        [Display(Name = "정렬순서")]
        public int SortOrder { get; set; }

        [Display(Name = "공개여부")]
        public bool IsPublished { get; set; } = true;

        public List<WorkImage> ExistingImages { get; set; } = new();
    }
}
