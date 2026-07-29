using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IFileRepository
    {
        Task<IEnumerable<FilesInfo>> GetAllAsync(CancellationToken token);
        Task<FilesInfo?> GetByIdAsync(Guid fileId, CancellationToken token);
        Task<FilesInfo> CreateAsync(FilesInfo filesInfo, CancellationToken token);
        Task<FilesInfo?> UpdateAsync(FilesInfo filesInfo, CancellationToken token);
        Task<bool> DeleteAsync(Guid fileId, CancellationToken token);
    }
}