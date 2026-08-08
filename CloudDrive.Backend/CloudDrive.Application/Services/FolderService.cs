using CloudDrive.Application.DTOs.FolderDTOs;
using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _repo;

        public FolderService(IFolderRepository repo)
        {
            _repo = repo;
        }

        public async Task<Folder> AddFolderAsync(Guid userId, AddFolderDto folderDto, CancellationToken token)
        {
            if (folderDto is null)
                throw new Exception("Request data is null");

            var folder = new Folder()
            {
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow,
                ParentFolderId = folderDto.ParentFolderId,
                Name = folderDto.Name,
            };
            await _repo.CreateAsync(folder, token);
            return folder;
        }

        public async Task DeleteFolderAsync(Guid userId, Guid folderId, CancellationToken token)
        {
            bool flag = await _repo.DeleteByIdAsync(userId, folderId, token);
            if (!flag)
            {
                throw new Exception("Failed to delete");
            }
        }

        public async Task<UpdateFolderDto> UpdateFolderAsync(Guid userId, UpdateFolderDto folderDto, CancellationToken token)
        {
            var updatefolder = new Folder()
            {
                Id = folderDto.Id,
                Name = folderDto.Name,
                ParentFolderId = folderDto.ParentFolderId,
                UpdatedAt = DateTime.UtcNow,
            };
            var updatedFolder = await _repo.UpdateAsync(userId, updatefolder, token);
            if (updatedFolder is null)
                throw new Exception("Failed to updated Folder");
            return folderDto;
        }
    }
}