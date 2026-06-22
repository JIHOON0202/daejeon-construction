using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DaejeonConstruction.Web.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions;

        public FileUploadService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _allowedExtensions = config.GetSection("FileUpload:AllowedExtensions").Get<string[]>()
                ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        }

        public bool IsAllowedImage(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(ext);
        }

        public async Task<string?> SaveAsync(IFormFile? file, string subFolder)
        {
            if (file == null || file.Length == 0) return null;
            if (!IsAllowedImage(file))
            {
                throw new InvalidOperationException($"허용되지 않는 파일 형식입니다: {file.FileName}");
            }

            var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", subFolder);
            Directory.CreateDirectory(uploadsRoot);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadsRoot, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{subFolder}/{fileName}";
        }

        public void Delete(string? webPath)
        {
            if (string.IsNullOrWhiteSpace(webPath)) return;

            var relative = webPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_env.WebRootPath, relative);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
