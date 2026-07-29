using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken token);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken token);
        Task<User> CreateAsync(User user, CancellationToken token);
        Task<User?> UpdateAsync(User user, CancellationToken token);
        Task<bool> DeleteByIdAsync(Guid userId, CancellationToken token);
    }
}