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

        public async Task<string> UploadFileAsync(string fileKey, Stream fileStream, string contentType, string userId)
        {
            var request = new PutObjectRequest
            {
                BucketName = "your-bucket-name",
                Key = fileKey,
                InputStream = fileStream,
                ContentType = contentType,
                Metadata = { ["UserId"] = userId }
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

        public async Task<List<FileMetadata>> ListFilesAsync(string userId)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = "your-bucket-name",
                Prefix = $"{userId}/"
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            var files = new List<FileMetadata>();

            foreach (var s3Object in response.S3Objects)
            {
                files.Add(new FileMetadata
                {
                    Key = s3Object.Key,
                    Size = s3Object.Size,
                    LastModified = s3Object.LastModified
                });
            }

            return files;
        }
    }
}
