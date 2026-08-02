using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IFolderRepository
    {
        Task<IEnumerable<Folder>> GetAllAsync(int page, int pageSize, CancellationToken token);
        Task<Folder?> GetByIdAsync(Guid fileId, CancellationToken token);
        Task<Folder> CreateAsync(Folder folder, CancellationToken token);
        Task<Folder?> UpdateAsync(Folder folder, CancellationToken token);
        Task<bool> DeleteByIdAsync(Guid folderId, CancellationToken token);
    }
}