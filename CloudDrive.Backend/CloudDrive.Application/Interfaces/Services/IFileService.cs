using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<FilesInfo> AddFileAsync(AddFileDto fileDto, CancellationToken token);
        Task DeleteFileAsync(Guid fileId, CancellationToken token);
        Task UpdateFileAsync(UpdateFileDto fileDto, CancellationToken token);
    }
}