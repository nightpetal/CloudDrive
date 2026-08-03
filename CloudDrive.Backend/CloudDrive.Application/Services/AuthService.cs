using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CloudDrive.Application.DTOs.UserDTOs;
using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CloudDrive.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _repo;
        public AuthService(IConfiguration config, IUserRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        public string CreateToken(Guid id, string email, string role)
        {
            var configuration = _config.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"]!));
            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Issuer"],
                audience: configuration["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(configuration["TokenTimeInMins"]!)),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Register(Register register, CancellationToken token)
        {
            var exisingUser = await _repo.GetByEmailAsync(register.Email, token);
            if (exisingUser != null)
                throw new Exception("User exists");
            var newUser = new User()
            {
                Email = register.Email,
                PasswordHash = string.Empty,
                Username = register.Username,
                StorageLimitBytes = 500 * 1024 * 1024,
                StorageUsed = 0,
                Role = "User",
                JoinedDate = DateTime.UtcNow
            };
            string hashedPassword = new PasswordHasher<User>().HashPassword(newUser, register.Password);
            newUser.PasswordHash = hashedPassword;
            var user = await _repo.CreateAsync(newUser, token);
            var jwtToken = CreateToken(user.Id, user.Email, user.Role);
            return jwtToken;
        }

        public async Task<string> Login(Login login, CancellationToken token)
        {
            if (login is null)
                throw new NoNullAllowedException("Login creds is null");
            var exisingUser = await _repo.GetByEmailAsync(login.Email, token);
            if (exisingUser is null)
                throw new Exception("User not found");

            var result = new PasswordHasher<User>().VerifyHashedPassword(
                exisingUser, exisingUser.PasswordHash, login.Password
                );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Password doesnt match");

            var jwtToken = CreateToken(exisingUser.Id, exisingUser.Email, exisingUser.Role);
            return jwtToken;
        }

    }
}