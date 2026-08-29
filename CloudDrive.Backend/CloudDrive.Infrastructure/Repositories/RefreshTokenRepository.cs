using CloudDrive.Application.Interfaces;
using CloudDrive.Domain.Entities;
using CloudDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> CreateAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(
                refreshToken,
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return refreshToken;
        }

        public async Task<RefreshToken?> GetByHashAsync(
            string tokenHash,
            CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(
                    t => t.TokenHash == tokenHash &&
                         t.IsActive,
                    cancellationToken);
        }

        public async Task<bool> RevokeAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(
                    t => t.Id == id,
                    cancellationToken);

            if (existingToken == null)
                return false;

            existingToken.IsActive = false;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
