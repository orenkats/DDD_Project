using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ITraderRepository _traderRepository;

        public FileService(IFileStorageService fileStorageService, ITraderRepository traderRepository)
        {
            _fileStorageService = fileStorageService;
            _traderRepository = traderRepository;
        }

        public async Task<string> GenerateAndUploadJsonFileAsync(Guid traderId)
        {
            var fileKey = $"{traderId}/reports/summary.json";

            if (await _fileStorageService.FileExistsAsync(fileKey))
            {
                return fileKey;
            }

            var orders = await _traderRepository.GetOrdersByTraderIdAsync(traderId);

            using var jsonStream = GenerateJsonReport(traderId, orders);

            return await _fileStorageService.UploadFileAsync(fileKey, jsonStream, "application/json", traderId);
        }

        public Task<Stream?> DownloadFileAsync(string fileKey)
        {
            return _fileStorageService.DownloadFileAsync(fileKey);
        }


        private Stream GenerateJsonReport(Guid traderId, List<StockOrder> orders)
        {
            var memoryStream = new MemoryStream();

            var report = new
            {
                TraderId = traderId,
                ReportDate = DateTime.UtcNow,
                Orders = orders.Select(o => new
                {
                    OrderId = o.Id,
                    Amount = o.Quantity * o.Price,
                    Date = o.CreatedAt
                }).ToList()
            };

            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            var json = System.Text.Json.JsonSerializer.Serialize(report, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            writer.Write(json);
            writer.Flush();

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
