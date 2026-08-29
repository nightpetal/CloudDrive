using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        public AuthService(
            IConfiguration config,
            IUserRepository repo,
            IRefreshTokenRepository refreshTokenRepo)
        {
            _config = config;
            _repo = repo;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public string CreateToken(Guid id, string email, string role)
        {
            var configuration = _config.GetSection("JWT");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
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
                    int.Parse(configuration["TokenTimeInMins"]!)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponse> Register(
            Register register,
            CancellationToken cancellationToken)
        {
            var existingUser = await _repo.GetByEmailAsync(
                register.Email,
                cancellationToken);

            if (existingUser != null)
                throw new Exception("User exists");

            var newUser = new User
            {
                Email = register.Email,
                PasswordHash = string.Empty,
                Username = register.Username,
                StorageLimitBytes = 500 * 1024 * 1024,
                StorageUsed = 0,
                Role = "User",
                JoinedDate = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<User>();

            newUser.PasswordHash = passwordHasher.HashPassword(
                newUser,
                register.Password);

            var user = await _repo.CreateAsync(
                newUser,
                cancellationToken);

            var accessToken = CreateToken(
                user.Id,
                user.Email,
                user.Role);

            var refreshToken = await CreateAndSaveRefreshToken(
                user.Id,
                cancellationToken);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> Login(
            Login login,
            CancellationToken cancellationToken)
        {
            if (login is null)
                throw new NoNullAllowedException("Login creds is null");

            var existingUser = await _repo.GetByEmailAsync(
                login.Email,
                cancellationToken);

            if (existingUser is null)
                throw new Exception("User not found");

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(
                existingUser,
                existingUser.PasswordHash,
                login.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Password doesnt match");

            var accessToken = CreateToken(
                existingUser.Id,
                existingUser.Email,
                existingUser.Role);

            var refreshToken = await CreateAndSaveRefreshToken(
                existingUser.Id,
                cancellationToken);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string HashRefreshToken(string refreshToken)
        {
            return Convert.ToBase64String(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(refreshToken)
                )
            );
        }

        private async Task<string> CreateAndSaveRefreshToken(
            Guid userId,
            CancellationToken cancellationToken)
        {
            // Generate a cryptographically secure random token.
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            // This is the token that will be returned to the client.
            var refreshToken = Convert.ToBase64String(randomBytes);

            // Store only the hash in the database.
            var tokenHash = HashRefreshToken(refreshToken);

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                TokenHash = tokenHash,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    int.Parse(
                        _config["JWT:RefreshTokenExpirationDays"]!
                    )
                ),
                IsActive = true
            };

            await _refreshTokenRepo.CreateAsync(
                refreshTokenEntity,
                cancellationToken);

            return refreshToken;
        }
        public async Task<AuthResponse> RefreshToken(
    string refreshToken,
    CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException(
                    "Refresh token is required.");

            // Hash the token received from the client
            var tokenHash = HashRefreshToken(refreshToken);

            // Find token in database
            var existingToken = await _refreshTokenRepo.GetByHashAsync(
                tokenHash,
                cancellationToken);

            if (existingToken == null)
                throw new UnauthorizedAccessException(
                    "Invalid refresh token.");

            // Check expiration
            if (existingToken.ExpiresAt <= DateTime.UtcNow)
            {
                await _refreshTokenRepo.RevokeAsync(
                    existingToken.Id,
                    cancellationToken);

                throw new UnauthorizedAccessException(
                    "Refresh token has expired.");
            }

            // Get the user
            var user = await _repo.GetByIdAsync(
                existingToken.UserId,
                cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException(
                    "User not found.");

            // Create new access token
            var accessToken = CreateToken(
                user.Id,
                user.Email,
                user.Role);

            // Rotate refresh token
            await _refreshTokenRepo.RevokeAsync(
                existingToken.Id,
                cancellationToken);

            var newRefreshToken = await CreateAndSaveRefreshToken(
                user.Id,
                cancellationToken);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }
        public async Task RevokeRefreshToken(
            string refreshToken,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException(
                    "Refresh token is required.");

            var tokenHash = HashRefreshToken(refreshToken);

            var existingToken = await _refreshTokenRepo.GetByHashAsync(
                tokenHash,
                cancellationToken);

            if (existingToken == null)
                throw new UnauthorizedAccessException(
                    "Invalid refresh token.");

            await _refreshTokenRepo.RevokeAsync(
                existingToken.Id,
                cancellationToken);
        }

    }
}
