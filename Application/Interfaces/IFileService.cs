using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFileService
    {
        Task<string> GenerateAndUploadJsonFileAsync(Guid traderId);
        Task<Stream?> DownloadFileAsync(string fileKey);
        
    }
}
