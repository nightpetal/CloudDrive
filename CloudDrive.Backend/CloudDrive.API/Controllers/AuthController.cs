using CloudDrive.Application.DTOs.UserDTOs;
using CloudDrive.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login([FromBody] Login login, CancellationToken token)
        {
            try
            {
                var jwtToken = await _auth.Login(login, token);
                return Ok(jwtToken);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("/register")]
        public async Task<ActionResult> Register([FromBody] Register register, CancellationToken token)
        {
            try
            {
                var jwtToken = await _auth.Register(register, token);
                return Ok(jwtToken);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}