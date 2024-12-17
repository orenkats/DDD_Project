using Amazon.S3;
using Amazon.S3.Model;
using Domain.Interfaces;
using Domain.Entities;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class S3FileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;

        public S3FileStorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<string> UploadFileAsync(string fileKey, Stream fileStream, string contentType, Guid traderId)
        {
            var request = new PutObjectRequest
            {
                BucketName = "your-bucket-name",
                Key = fileKey,
                InputStream = fileStream,
                ContentType = contentType,
                Metadata = { ["traderId"] = traderId.ToString() }
            };

            await _s3Client.PutObjectAsync(request);
            return fileKey;
        }

        public async Task<Stream?> DownloadFileAsync(string fileKey)
        {
            try
            {
                var response = await _s3Client.GetObjectAsync("your-bucket-name", fileKey);
                return response.ResponseStream;
            }
            catch (AmazonS3Exception)
            {
                return null;
            }
        }

        public async Task<bool> FileExistsAsync(string fileKey)
        {
            try
            {
                await _s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest
                {
                    BucketName = "your-bucket-name",
                    Key = fileKey
                });
                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
    }
}
