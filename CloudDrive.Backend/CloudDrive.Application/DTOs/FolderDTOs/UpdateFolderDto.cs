namespace CloudDrive.Application.DTOs.FolderDTOs
{
    public class UpdateFolderDto
    {
        public Guid Id { get; set; }
        public Guid? ParentFolderId { get; set; }
        public required string Name { get; set; }
    }
}