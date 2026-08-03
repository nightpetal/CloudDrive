namespace CloudDrive.Application.DTOs.UserDTOs
{
    public class Register
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}