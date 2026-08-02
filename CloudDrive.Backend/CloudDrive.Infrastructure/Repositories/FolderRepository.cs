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

        public async Task<bool> DeleteByIdAsync(Guid folderId, CancellationToken token)
        {
            var existingFolder = await _context.Folders.FindAsync(folderId);
            if (existingFolder is null)
                return false;
            _context.Folders.Remove(existingFolder);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<Folder>> GetAllAsync(int page, int pageSize, CancellationToken token)
        {
            return await _context.Folders
            .OrderBy(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);
        }

        public async Task<Folder?> GetByIdAsync(Guid folderId, CancellationToken token)
        {
            return await _context.Folders.FindAsync(folderId);
        }

        public async Task<Folder?> UpdateAsync(Folder folder, CancellationToken token)
        {
            var existingFolder = await _context.Folders.FindAsync(folder.Id);
            if (existingFolder is null)
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