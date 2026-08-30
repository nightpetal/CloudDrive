namespace CloudDrive.Application.DTOs.UserDTOs
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public int StorageLimitBytes { get; set; }
        public int StorageUsedBytes { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
