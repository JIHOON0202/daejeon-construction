using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DaejeonConstruction.Web.Models.ViewModels
{
    public class EstimateCreateViewModel
    {
        [Required(ErrorMessage = "성함을 입력해 주세요.")]
        [Display(Name = "성함")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "연락처를 입력해 주세요.")]
        [Display(Name = "연락처")]
        [MaxLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "시공지역")]
        [MaxLength(100)]
        public string? Area { get; set; }

        [Display(Name = "시공품목")]
        [MaxLength(50)]
        public string? ServiceType { get; set; }

        [Display(Name = "문의내용")]
        [MaxLength(2000)]
        public string? Message { get; set; }

        [Display(Name = "사진첨부")]
        public List<IFormFile>? Photos { get; set; }
    }
}
