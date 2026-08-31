using CloudDrive.Application.DTOs.UserDTOs;
using CloudDrive.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] Login login, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _auth.Login(
                    login,
                    cancellationToken);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("/register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] Register register, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _auth.Register(
                    register,
                    cancellationToken);

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _auth.RefreshToken(
                    request.RefreshToken,
                    cancellationToken);

                return Ok(response);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(
            [FromBody] string refreshToken,
            CancellationToken cancellationToken)
        {
            try
            {
                await _auth.RevokeRefreshToken(
                    refreshToken,
                    cancellationToken);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
