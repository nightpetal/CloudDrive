// Infrastructure/DependencyInjection.cs
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Infrastructure.Data;
using CloudDrive.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudDrive.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IStorageService, MinioStorageService>();

            return services;
        }
    }
}