using CloudDrive.API.Extensions;
using CloudDrive.Application.DTOs.UserDTOs;
using CloudDrive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserProfileDto>> Profile(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var user = await _repo.GetByIdAsync(userId, cancellationToken);

            if (user == null)
                return NotFound("User not found");

            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                StorageLimitBytes = user.StorageLimitBytes,
                StorageUsedBytes = user.StorageUsed,
                JoinedDate = user.JoinedDate
            };

            return Ok(profileDto);
        }
    }
}