using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken);

        Task<RefreshToken?> GetByHashAsync(
            string tokenHash,
            CancellationToken cancellationToken);

        Task<bool> RevokeAsync(
            Guid id,
            CancellationToken cancellationToken);
    }
}
