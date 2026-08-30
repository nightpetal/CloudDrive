using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<FilesInfo> AddFileAsync(Guid userId, AddFileDto fileDto, CancellationToken token);
        Task<FilesInfo> UploadFileAsync(Guid userId, string fileName, Stream fileStream, string mimeType, Guid? folderId, CancellationToken token);
        Task<Stream> DownloadFileAsync(string storageKey, CancellationToken token);
        Task DeleteFileAsync(Guid userId, Guid fileId, CancellationToken token);
        Task UpdateFileAsync(Guid userId, UpdateFileDto fileDto, CancellationToken token);
    }
}