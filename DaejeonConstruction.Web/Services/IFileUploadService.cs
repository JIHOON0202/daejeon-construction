using Microsoft.AspNetCore.Http;

namespace DaejeonConstruction.Web.Services
{
    public interface IFileUploadService
    {
        /// <summary>
        /// 업로드 파일을 wwwroot/uploads/{subFolder} 에 저장하고 "/uploads/{subFolder}/{file}" 형태의 웹 경로를 반환한다.
        /// </summary>
        Task<string?> SaveAsync(IFormFile? file, string subFolder);

        void Delete(string? webPath);

        bool IsAllowedImage(IFormFile file);
    }
}
