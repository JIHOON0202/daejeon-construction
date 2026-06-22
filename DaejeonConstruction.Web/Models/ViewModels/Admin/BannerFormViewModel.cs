using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DaejeonConstruction.Web.Models.ViewModels.Admin
{
    public class BannerFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "작은 문구를 입력해 주세요.")]
        [Display(Name = "작은 문구(Eyebrow)")]
        public string Eyebrow { get; set; } = string.Empty;

        [Required(ErrorMessage = "제목을 입력해 주세요.")]
        [Display(Name = "제목")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "부제")]
        public string? SubText { get; set; }

        [Display(Name = "버튼 텍스트")]
        public string ButtonText { get; set; } = "견적문의 바로가기 →";

        [Display(Name = "버튼 링크")]
        public string ButtonLink { get; set; } = "#quote";

        [Display(Name = "배너 이미지")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }

        [Display(Name = "정렬순서")]
        public int SortOrder { get; set; }

        [Display(Name = "사용여부")]
        public bool IsActive { get; set; } = true;
    }
}
