using Microsoft.AspNetCore.Http;
using HouseBroker.Application.Interfaces;

namespace HouseBroker.Application.Services
{
    public class FileService : IFileService
    {
        public async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            if (file == null) return "";

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{folderName}");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folderName}/{fileName}";
        }
    }
}
