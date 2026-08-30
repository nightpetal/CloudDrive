using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepo;
        private readonly IStorageService _storageService;
        private const string BucketName = "clouddrive";

        public FileService(IFileRepository fileRepo, IStorageService storageService)
        {
            _fileRepo = fileRepo;
            _storageService = storageService;
        }

        public async Task<FilesInfo> AddFileAsync(Guid userId, AddFileDto fileDto, CancellationToken token)
        {
            if (fileDto is null)
                throw new InvalidDataException("Dto is null");
            var file = new FilesInfo()
            {
                OwnerId = userId,
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

        public async Task<FilesInfo> UploadFileAsync(Guid userId, string fileName, Stream fileStream, string mimeType, Guid? folderId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new InvalidOperationException("File name is required");

            if (fileStream == null || fileStream.Length == 0)
                throw new InvalidOperationException("File stream is empty");

            // Generate unique storage key
            var storageKey = $"{userId}/{DateTime.UtcNow:yyyy-MM-dd}/{Guid.NewGuid()}_{fileName}";
            var extension = Path.GetExtension(fileName);

            try
            {
                // Upload to MinIO
                await _storageService.UploadFileAsync(fileStream, BucketName, storageKey, mimeType, token);

                // Create file record in database
                var file = new FilesInfo()
                {
                    OwnerId = userId,
                    FolderId = folderId,
                    Extension = extension,
                    MimeType = mimeType,
                    OrginalName = fileName,
                    StorageKey = storageKey,
                    SizeBytes = (int)fileStream.Length,
                    CreatedAt = DateTime.UtcNow,
                };

                await _fileRepo.CreateAsync(file, token);
                return file;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to upload file: {ex.Message}", ex);
            }
        }

        public async Task<Stream> DownloadFileAsync(string storageKey, CancellationToken token)
        {
            if (string.IsNullOrEmpty(storageKey))
                throw new InvalidOperationException("Storage key is required");

            try
            {
                return await _storageService.DownloadFileAsync(BucketName, storageKey, token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to download file: {ex.Message}", ex);
            }
        }

        public async Task DeleteFileAsync(Guid userId, Guid fileId, CancellationToken token)
        {
            bool flag = await _fileRepo.DeleteAsync(userId, fileId, token);
            if (!flag)
            {
                throw new Exception("Failed to delete");
            }
        }

        public async Task UpdateFileAsync(Guid userId, UpdateFileDto fileDto, CancellationToken token)
        {
            var file = new FilesInfo()
            {
                FolderId = fileDto.FolderId,
                OrginalName = fileDto.OrginalName,
                StorageKey = fileDto.StorageKey,
                MimeType = fileDto.MimeType,
                Extension = fileDto.Extension,
                UpdatedAt = DateTime.UtcNow
            };
            var updatedFile = await _fileRepo.UpdateAsync(userId, file, token);
            if (updatedFile is null)
                throw new Exception("Failed to updatedFile");
        }
    }
}