using CloudDrive.Application.DTOs.UserDTOs;

namespace CloudDrive.Application.Interfaces.Services
{
    public interface IAuthService
    {
        string CreateToken(Guid id, string email, string role);
        Task<string> Login(Login login, CancellationToken token);
        Task<string> Register(Register register, CancellationToken token);
    }
}