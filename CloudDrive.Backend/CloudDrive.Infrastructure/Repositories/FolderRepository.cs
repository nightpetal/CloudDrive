using CloudDrive.Application.Interfaces;
using CloudDrive.Domain.Entities;
using CloudDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Infrastructure.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly AppDbContext _context;

        public FolderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Folder> CreateAsync(Folder folder, CancellationToken token)
        {
            await _context.Folders.AddAsync(folder, token);
            await _context.SaveChangesAsync(token);
            return folder;
        }

        public async Task<bool> DeleteByIdAsync(Guid userId, Guid folderId, CancellationToken token)
        {
            var existingFolder = await _context.Folders.FindAsync(folderId);
            if (existingFolder is null || existingFolder.OwnerId != userId)
                return false;
            _context.Folders.Remove(existingFolder);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<Folder>> GetAllAsync(Guid userId, int page, int pageSize, CancellationToken token)
        {
            return await _context.Folders
            .Where(f => f.OwnerId == userId)
            .OrderBy(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
        }

        public async Task<Folder?> GetByIdAsync(Guid userId, Guid folderId, CancellationToken token)
        {
            return await _context.Folders.FirstOrDefaultAsync(f => f.OwnerId == userId && f.Id == folderId);
        }

        public async Task<Folder?> UpdateAsync(Guid userId, Folder folder, CancellationToken token)
        {
            var existingFolder = await _context.Folders.FindAsync(folder.Id);
            if (existingFolder is null || existingFolder.OwnerId != userId)
                return null;
            existingFolder.Name = folder.Name;
            existingFolder.DeletedAt = folder.DeletedAt;
            existingFolder.ParentFolderId = folder.ParentFolderId;
            existingFolder.UpdatedAt = folder.UpdatedAt;

            await _context.SaveChangesAsync(token);
            return existingFolder;
        }
    }
}