using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<string> GenerateAndUploadFileAsync(string userId);
        Task<Stream?> DownloadFileAsync(string fileKey);
        Task<List<FileMetadata>> ListUserFilesAsync(string userId);
    }
}
