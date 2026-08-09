using CloudDrive.Application.DTOs.Response;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IFileRepository
    {
        Task<PagedResult<FilesInfo>> GetAllAsync(Guid userId, Guid? folderId, int page, int pageSize, CancellationToken token);
        Task<FilesInfo?> GetByIdAsync(Guid userId, Guid fileId, CancellationToken token);
        Task<FilesInfo> CreateAsync(FilesInfo filesInfo, CancellationToken token);
        Task<FilesInfo?> UpdateAsync(Guid userId, FilesInfo filesInfo, CancellationToken token);
        Task<bool> DeleteAsync(Guid userId, Guid fileId, CancellationToken token);
    }
}