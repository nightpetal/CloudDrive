using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;

namespace CloudDrive.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepo;
        private readonly IStorageService _storageService;
        private readonly IUserRepository _userRepo;
        private const string BucketName = "clouddrive";

        public FileService(IFileRepository fileRepo, IStorageService storageService, IUserRepository userRepo)
        {
            _fileRepo = fileRepo;
            _storageService = storageService;
            _userRepo = userRepo;
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

            // Update user's storage usage
            var user = await _userRepo.GetByIdAsync(userId, token);
            if (user != null)
            {
                user.StorageUsed += fileDto.SizeBytes;
                await _userRepo.UpdateAsync(user, token);
            }

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
            var fileSizeBytes = (int)fileStream.Length;

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
                    SizeBytes = fileSizeBytes,
                    CreatedAt = DateTime.UtcNow,
                };

                await _fileRepo.CreateAsync(file, token);

                // Update user's storage usage
                var user = await _userRepo.GetByIdAsync(userId, token);
                if (user != null)
                {
                    user.StorageUsed += fileSizeBytes;
                    await _userRepo.UpdateAsync(user, token);
                }

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
            // Get the file before deletion to retrieve its size
            var file = await _fileRepo.GetByIdAsync(userId, fileId, token);
            if (file == null)
            {
                throw new Exception("File not found");
            }

            bool flag = await _fileRepo.DeleteAsync(userId, fileId, token);
            if (!flag)
            {
                throw new Exception("Failed to delete");
            }

            // Update user's storage usage after successful deletion
            var user = await _userRepo.GetByIdAsync(userId, token);
            if (user != null)
            {
                user.StorageUsed -= file.SizeBytes;
                // Ensure StorageUsed doesn't go negative
                if (user.StorageUsed < 0)
                    user.StorageUsed = 0;
                await _userRepo.UpdateAsync(user, token);
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