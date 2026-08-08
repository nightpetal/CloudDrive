using CloudDrive.Application.DTOs.Request;
using CloudDrive.Domain.Entities;
using System.IO;

namespace CloudDrive.Infrastructure.Mapper
{
    public static class FileMapper
    {
        public static FileRequest MapFile(this FilesInfo file)
        {
            var fileRequest = new FileRequest
            {
                Id = file.Id,
                Extension = file.Extension,
                SizeBytes = file.SizeBytes,
                OrginalName = file.OrginalName,
                UpdatedAt = file.UpdatedAt ?? file.CreatedAt
            };

            return fileRequest;
        }
    }
}
