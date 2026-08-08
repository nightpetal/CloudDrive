namespace CloudDrive.Application.DTOs.Request
{
    public class FileRequest
    {
        public Guid Id { get; set; }
        public required string OrginalName { get; set; }
        public required string Extension { get; set; }
        public int SizeBytes { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}