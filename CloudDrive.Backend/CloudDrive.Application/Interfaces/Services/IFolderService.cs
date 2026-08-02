using CloudDrive.Application.DTOs.FolderDTOs;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces.Services
{
    public interface IFolderService
    {
        Task<Folder> AddFolderAsync(AddFolderDto folderDto, CancellationToken token);
        Task DeleteFolderAsync(Guid folderId, CancellationToken token);
        Task<UpdateFolderDto> UpdateFolderAsync(UpdateFolderDto folderDto, CancellationToken token);
    }
}