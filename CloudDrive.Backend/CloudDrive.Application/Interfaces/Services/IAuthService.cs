using CloudDrive.Application.DTOs.UserDTOs;

namespace CloudDrive.Application.Interfaces.Services
{
    public interface IAuthService
    {
        string CreateToken(Guid id, string email, string role);

        Task<AuthResponse> Register(
            Register register,
            CancellationToken cancellationToken);

        Task<AuthResponse> Login(
            Login login,
            CancellationToken cancellationToken);

        Task<AuthResponse> RefreshToken(
            string refreshToken,
            CancellationToken cancellationToken);

        Task RevokeRefreshToken(
            string refreshToken,
            CancellationToken cancellationToken);
    }
}
