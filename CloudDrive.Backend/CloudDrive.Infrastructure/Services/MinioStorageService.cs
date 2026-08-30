using CloudDrive.Application.Interfaces.Services;
using Minio;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;

namespace CloudDrive.Infrastructure.Services
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;

        public MinioStorageService(IConfiguration configuration)
        {
            var endpoint = configuration["Minio:Endpoint"] ?? throw new ArgumentNullException("Minio:Endpoint");
            var accessKey = configuration["Minio:AccessKey"] ?? throw new ArgumentNullException("Minio:AccessKey");
            var secretKey = configuration["Minio:SecretKey"] ?? throw new ArgumentNullException("Minio:SecretKey");
            var useSSL = bool.Parse(configuration["Minio:UseSSL"] ?? "false");
            _bucketName = configuration["Minio:BucketName"] ?? throw new ArgumentNullException("Minio:BucketName");

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(useSSL)
                .Build();
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string bucketName, string objectName, string contentType, CancellationToken cancellationToken = default)
        {
            try
            {
                // Ensure bucket exists
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
                bool bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

                if (!bucketExists)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                }

                // Reset stream position if possible
                if (fileStream.CanSeek)
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                }

                // Upload the file
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);

                await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                return objectName;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to upload file to MinIO: {ex.Message}", ex);
            }
        }

        public async Task<Stream> DownloadFileAsync(string bucketName, string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);

                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to download file from MinIO: {ex.Message}", ex);
            }
        }

        public async Task DeleteFileAsync(string bucketName, string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete file from MinIO: {ex.Message}", ex);
            }
        }

        public async Task<bool> BucketExistsAsync(string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
                return await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
            }
            catch
            {
                return false;
            }
        }
    }
}
