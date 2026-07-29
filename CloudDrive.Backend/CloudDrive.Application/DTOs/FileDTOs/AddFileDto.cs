public class AddFileDto
{
    public Guid OwnerId { get; set; }
    public Guid? FolderId { get; set; }
    public int SizeBytes { get; set; }
    public required string OrginalName { get; set; }
    public required string StorageKey { get; set; }
    public required string MimeType { get; set; }
    public required string Extension { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}