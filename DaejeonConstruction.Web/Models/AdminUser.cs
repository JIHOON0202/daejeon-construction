using System.ComponentModel.DataAnnotations;

namespace DaejeonConstruction.Web.Models
{
    /// <summary>
    /// 관리자 계정 (ADMIN_USER)
    /// </summary>
    public class AdminUser
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? DisplayName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLoginAt { get; set; }
    }
}
