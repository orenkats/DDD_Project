namespace Application.Services
{
    using Domain.Interfaces;
    using Domain.Entities;
    using Application.Interfaces;
    using System.Text;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class FileService : IFileService
    {
        private readonly IFileStorageService _fileStorageService;

        public FileService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<string> GenerateAndUploadFileAsync(string userId)
        {
            var fileKey = $"{userId}/reports/summary.pdf";
            if (await _fileStorageService.FileExistsAsync(fileKey))
            {
                return fileKey;
            }

            using var reportStream = GenerateReport(userId);
            return await _fileStorageService.UploadFileAsync(fileKey, reportStream, "application/pdf", userId);
        }

        public Task<Stream?> DownloadFileAsync(string fileKey)
        {
            return _fileStorageService.DownloadFileAsync(fileKey);
        }

        public Task<List<FileMetadata>> ListUserFilesAsync(string userId)
        {
            return _fileStorageService.ListFilesAsync(userId);
        }

        private Stream GenerateReport(string userId)
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            writer.WriteLine($"Yearly Summary Report for User {userId}");
            writer.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
