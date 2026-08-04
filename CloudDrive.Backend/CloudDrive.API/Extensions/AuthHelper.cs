using System.Security.Claims;

namespace CloudDrive.API.Extensions
{
    public static class AuthHelper
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(
                user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}