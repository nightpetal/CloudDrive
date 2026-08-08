public class AddFolderDto
{
    public Guid? ParentFolderId { get; set; }
    public required string Name { get; set; }
}