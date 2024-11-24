using Microsoft.AspNetCore.Http;

namespace CommonBoilerPlateEight.Domain.Helper
{
    public interface IFileUploaderService
    {
        Task<string> SaveFileAsync(IFormFile file, string directoryName);
        void RemoveFile(string filePath);
        void ValidateImageFiles(List<IFormFile> files);
        string GetImageBaseUrl();
       
    }
}
