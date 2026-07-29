namespace CloudDrive.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public int StorageLimitBytes { get; set; }
        public int StorageUsed { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}