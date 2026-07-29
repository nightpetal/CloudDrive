using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepo;

        public FileService(IFileRepository fileRepo)
        {
            _fileRepo = fileRepo;
        }

        public async Task<FilesInfo> AddFileAsync(AddFileDto fileDto, CancellationToken token)
        {
            if (fileDto is null)
                throw new InvalidDataException("Dto is null");
            var file = new FilesInfo()
            {
                OwnerId = fileDto.OwnerId,
                FolderId = fileDto.FolderId,
                Extension = fileDto.Extension,
                MimeType = fileDto.MimeType,
                OrginalName = fileDto.OrginalName,
                StorageKey = fileDto.StorageKey,
                SizeBytes = fileDto.SizeBytes,
                CreatedAt = DateTime.UtcNow,
            };

            await _fileRepo.CreateAsync(file, token);
            return file;
        }

        public async Task DeleteFileAsync(Guid fileId, CancellationToken token)
        {
            bool flag = await _fileRepo.DeleteAsync(fileId, token);
            if (!flag)
            {
                throw new Exception("Failed to delete");
            }
        }

        public async Task UpdateFileAsync(UpdateFileDto fileDto, CancellationToken token)
        {
            var file = new FilesInfo()
            {
                FolderId = fileDto.FolderId,
                OrginalName = fileDto.OrginalName,
                StorageKey = fileDto.StorageKey,
                MimeType = fileDto.MimeType,
                Extension = fileDto.Extension
            };
            var updatedFile = await _fileRepo.UpdateAsync(file, token);
            if (updatedFile is null)
                throw new Exception("Failed to updatedFile");
        }
    }
}