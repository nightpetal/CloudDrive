namespace CloudDrive.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string bucketName, string objectName, string contentType, CancellationToken cancellationToken = default);
        Task<Stream> DownloadFileAsync(string bucketName, string objectName, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string bucketName, string objectName, CancellationToken cancellationToken = default);
        Task<bool> BucketExistsAsync(string bucketName, CancellationToken cancellationToken = default);
    }
}
