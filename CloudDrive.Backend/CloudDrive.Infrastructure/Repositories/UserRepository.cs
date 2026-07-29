using CloudDrive.Application.Interfaces;
using CloudDrive.Domain.Entities;
using CloudDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateAsync(User user, CancellationToken token)
        {
            await _context.Users.AddAsync(user, token);
            await _context.SaveChangesAsync(token);
            return user;
        }

        public async Task<bool> DeleteByIdAsync(Guid userId, CancellationToken token)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken token)
        {
            return await _context.Users.ToListAsync(token);
        }

        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken token)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> UpdateAsync(User user, CancellationToken token)
        {
            var realUser = await _context.Users.FindAsync(user.Id);
            if (realUser is null)
                return null;
            realUser.Username = user.Username;
            realUser.Email = user.Email;
            realUser.PasswordHash = user.PasswordHash;
            realUser.StorageUsed = user.StorageUsed;
            realUser.StorageLimitBytes = user.StorageLimitBytes;

            await _context.SaveChangesAsync(token);

            return realUser;
        }
    }
}