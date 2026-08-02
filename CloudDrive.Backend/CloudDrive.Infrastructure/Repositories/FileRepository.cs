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

        public async Task<bool> DeleteAsync(Guid fileId, CancellationToken token)
        {
            var existingFile = await _context.FilesInfos.FindAsync(fileId);
            if (existingFile is null)
                return false;
            _context.FilesInfos.Remove(existingFile);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<FilesInfo>> GetAllAsync(int page, int pageSize, CancellationToken token)
        {
            return await _context.FilesInfos
            .OrderBy(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
        }

        public async Task<FilesInfo?> GetByIdAsync(Guid fileId, CancellationToken token)
        {
            return await _context.FilesInfos.FindAsync(fileId);
        }

        public async Task<FilesInfo?> UpdateAsync(FilesInfo filesInfo, CancellationToken token)
        {
            var existingFile = await _context.FilesInfos.FindAsync(filesInfo.Id);
            if (existingFile is null)
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