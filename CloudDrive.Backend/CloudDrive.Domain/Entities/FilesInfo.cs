namespace CloudDrive.Domain.Entities
{
    public class FilesInfo
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid? FolderId { get; set; }
        public required string OrginalName { get; set; }
        public required string StorageKey { get; set; }
        public required string Extension { get; set; }
        public required string MimeType { get; set; }
        public int SizeBytes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}