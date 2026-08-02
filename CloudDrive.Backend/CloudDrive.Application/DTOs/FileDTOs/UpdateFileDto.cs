public class UpdateFileDto
{
    public Guid? FolderId { get; set; }
    public required string OrginalName { get; set; }
    public required string StorageKey { get; set; }
    public required string MimeType { get; set; }
    public required string Extension { get; set; }
}