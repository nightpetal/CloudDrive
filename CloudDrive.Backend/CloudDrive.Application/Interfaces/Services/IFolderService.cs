using CloudDrive.Application.DTOs.FolderDTOs;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Interfaces.Services
{
    public interface IFolderService
    {
        Task<Folder> AddFolderAsync(Guid userId, AddFolderDto folderDto, CancellationToken token);
        Task DeleteFolderAsync(Guid userId, Guid folderId, CancellationToken token);
        Task<UpdateFolderDto> UpdateFolderAsync(Guid userId, UpdateFolderDto folderDto, CancellationToken token);
    }
}