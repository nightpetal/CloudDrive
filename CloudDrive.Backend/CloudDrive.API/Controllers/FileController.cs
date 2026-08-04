using CloudDrive.API.Extensions;
using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(await _repo.GetAllAsync(userId, page, pageSize, token));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FilesInfo>> GetFileById(Guid id, CancellationToken token)
        {
            var userId = User.GetUserId();
            var file = await _repo.GetByIdAsync(userId, id, token);

            if (file == null)
                return NotFound();

            return Ok(file);
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
                await _service.DeleteFileAsync(id, token);
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