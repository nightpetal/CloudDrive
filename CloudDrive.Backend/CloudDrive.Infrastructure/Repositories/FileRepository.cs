using CloudDrive.Application.Interfaces;
using CloudDrive.Domain.Entities;
using CloudDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FilesInfo> CreateAsync(FilesInfo filesInfo, CancellationToken token)
        {
            await _context.FilesInfos.AddAsync(filesInfo, token);
            await _context.SaveChangesAsync(token);
            return filesInfo;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid fileId, CancellationToken token)
        {
            var existingFile = await _context.FilesInfos.FindAsync(fileId);
            if (existingFile is null || existingFile.OwnerId != userId)
                return false;
            _context.FilesInfos.Remove(existingFile);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<FilesInfo>> GetAllAsync(Guid userId, int page, int pageSize, CancellationToken token)
        {
            return await _context.FilesInfos
            .Where(f => f.OwnerId == userId)
            .OrderBy(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
        }

        public async Task<FilesInfo?> GetByIdAsync(Guid userId, Guid fileId, CancellationToken token)
        {
            return await _context.FilesInfos.FirstOrDefaultAsync(f => f.Id == fileId && f.OwnerId == userId, token);
        }

        public async Task<FilesInfo?> UpdateAsync(Guid userId, FilesInfo filesInfo, CancellationToken token)
        {
            var existingFile = await _context.FilesInfos.FindAsync(filesInfo.Id);
            if (existingFile is null || existingFile.OwnerId != userId)
                return null;
            existingFile.OrginalName = filesInfo.OrginalName;
            existingFile.Extension = filesInfo.Extension;
            existingFile.CreatedAt = filesInfo.CreatedAt;
            existingFile.DeletedAt = filesInfo.DeletedAt;
            existingFile.MimeType = filesInfo.MimeType;
            existingFile.SizeBytes = filesInfo.SizeBytes;

            await _context.SaveChangesAsync(token);
            return existingFile;
        }
    }
}