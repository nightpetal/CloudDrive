using CloudDrive.API.Extensions;
using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;
using CloudDrive.Infrastructure.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]

    public class FileController : ControllerBase
    {
        private readonly IFileService _service;
        private readonly IFileRepository _repo;

        public FileController(IFileService service, IFileRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<FilesInfo>> GetFile(CancellationToken token, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var userId = User.GetUserId();
            var files = await _repo.GetAllAsync(userId, page, pageSize, token);
            var dto = files.Select(
                file => file.MapFile()
                ).ToList();
            return Ok(dto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FilesInfo>> GetFileById(Guid id, CancellationToken token)
        {
            var userId = User.GetUserId();
            var file = await _repo.GetByIdAsync(userId, id, token);

            if (file == null)
                return NotFound();

            return Ok(file.MapFile());
        }

        [HttpPost]
        public async Task<ActionResult<FilesInfo>> AddFile([FromBody] AddFileDto addFile, CancellationToken token)
        {
            var userId = User.GetUserId();
            var filesInfo = await _service.AddFileAsync(userId, addFile, token);
            return CreatedAtAction(
                nameof(GetFile),
                new { id = filesInfo.Id },
                filesInfo
            );
        }

        [HttpDelete]
        public async Task<ActionResult<FilesInfo>> DeleteFile([FromQuery] Guid id, CancellationToken token)
        {
            try
            {
                var userId = User.GetUserId();
                await _service.DeleteFileAsync(userId, id, token);
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }
    }

}