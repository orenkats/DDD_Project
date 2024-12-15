
namespace Domain.Interfaces
{
    using System.IO;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(string fileKey, Stream fileStream, string contentType, string userId);
        Task<Stream?> DownloadFileAsync(string fileKey);
        Task<bool> FileExistsAsync(string fileKey);
        Task<List<FileMetadata>> ListFilesAsync(string userId);
    }
}
