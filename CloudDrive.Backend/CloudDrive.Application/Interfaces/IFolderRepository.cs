using CloudDrive.Application.DTOs.Response;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IFolderRepository
    {
        Task<PagedResult<Folder>> GetAllAsync(Guid userId, int page, int pageSize, CancellationToken token);
        Task<Folder?> GetByIdAsync(Guid userId, Guid fileId, CancellationToken token);
        Task<Folder> CreateAsync(Folder folder, CancellationToken token);
        Task<Folder?> UpdateAsync(Guid userId, Folder folder, CancellationToken token);
        Task<bool> DeleteByIdAsync(Guid userId, Guid folderId, CancellationToken token);
    }
}