using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IFileRepository
    {
        Task<IEnumerable<FilesInfo>> GetAllAsync(Guid userId, int page, int pageSize, CancellationToken token);
        Task<FilesInfo?> GetByIdAsync(Guid userId, Guid fileId, CancellationToken token);
        Task<FilesInfo> CreateAsync(FilesInfo filesInfo, CancellationToken token);
        Task<FilesInfo?> UpdateAsync(FilesInfo filesInfo, CancellationToken token);
        Task<bool> DeleteAsync(Guid fileId, CancellationToken token);
    }
}