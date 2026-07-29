using CloudDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FilesInfo> FilesInfos { get; set; }
    }
}