using CloudDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudDrive.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FilesInfo> FilesInfos { get; set; }
    }
}

// dotnet ef migrations add <name> --project CloudDrive.Infrastructure --startup-project CloudDrive.API
// dotnet ef database update --project CloudDrive.Infrastructure --startup-project CloudDrive.API