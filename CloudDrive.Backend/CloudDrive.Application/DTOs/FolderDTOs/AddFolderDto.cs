public class AddFolderDto
{
    public Guid OwnerId { get; set; }
    public Guid? ParentFolderId { get; set; }
    public required string Name { get; set; }
}