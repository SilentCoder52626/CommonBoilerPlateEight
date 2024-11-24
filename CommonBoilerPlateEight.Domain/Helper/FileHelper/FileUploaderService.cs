using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CommonBoilerPlateEight.Domain.Exceptions;

namespace CommonBoilerPlateEight.Domain.Helper
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly string _rootPath;
        private readonly string _defaultFolder = "Uploads";
        private readonly string _baseImageUrl;
        public FileUploaderService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            var fileStorageSettings = configuration.GetSection("FileStorageSettings").Get<FileStorageSettings>();
            _rootPath = fileStorageSettings?.RootPath ?? webHostEnvironment.WebRootPath;
            _baseImageUrl = configuration.GetSection("ImageBaseUrl").Value;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string directoryName)
        {
            if (file == null || file.Length == 0) throw new ArgumentNullException(nameof(file));
            ValidateImageFiles(new List<IFormFile> { file });
            // Ensure the Images directory exists
            var imagesPath = Path.Combine(_rootPath, _defaultFolder);
            if (!Directory.Exists(imagesPath))
                Directory.CreateDirectory(imagesPath);

            // Create the subdirectory under Images
            var directoryPath = Path.Combine(imagesPath, directoryName);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // Generate a unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(directoryPath, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(_defaultFolder, directoryName, uniqueFileName);
        }

        public void RemoveFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fullPath = Path.Combine(_rootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public void ValidateImageFiles(List<IFormFile> files)
        {
            if (files.Any())
            {
                var imageFormatList = files.Select(a => a.ContentType).ToList();
                var fileSizeList = files.Select(a => a.Length).ToList();
                ValidateImageType(imageFormatList);
                ValidateImageSize(fileSizeList);
            }
        }

        private void ValidateImageType(List<string> imagesTypes)
        {
            string[] allowedImageTypes = { "image/jpeg", "image/png", "image/webp" };
            if (imagesTypes.Any(a => !allowedImageTypes.Contains(a))) throw new CustomException("Invalid Image Format. Only png,jpg,webp format supported");

        }

        private void ValidateImageSize(List<long> fileSizes)
        {
            const long maxFileSize = 5 * 1024 * 1024; // 3 MB in bytes
            if (fileSizes.Any(size => size > maxFileSize))
            {
                throw new CustomException("File size exceeds the 3 MB limit.");
            }
        }

        public string GetImageBaseUrl()
        {
            return _baseImageUrl;
        }
    }

    public class FileStorageSettings
    {
        public string RootPath { get; set; }
    }
}
