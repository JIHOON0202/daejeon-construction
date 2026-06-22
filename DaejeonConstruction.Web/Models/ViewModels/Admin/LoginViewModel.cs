using System.ComponentModel.DataAnnotations;

namespace DaejeonConstruction.Web.Models.ViewModels.Admin
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "아이디를 입력해 주세요.")]
        [Display(Name = "아이디")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "비밀번호를 입력해 주세요.")]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
