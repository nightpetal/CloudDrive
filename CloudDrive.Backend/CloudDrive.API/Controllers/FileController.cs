using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FileController : ControllerBase
    {
        private readonly IFileService _service;
        private readonly IFileRepository _repo;

        public FileController(IFileService service, IFileRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpGet("all")]
        public async Task<ActionResult<FilesInfo>> GetFile(CancellationToken token)
        {
            return Ok(await _repo.GetAllAsync(token));
        }

        [HttpGet]
        public async Task<ActionResult<FilesInfo>> GetFile([FromQuery] Guid id, CancellationToken token)
        {
            var file = await _repo.GetByIdAsync(id, token);

            if (file == null)
                return NotFound();

            return Ok(file);
        }

        [HttpPost]
        public async Task<ActionResult<FilesInfo>> AddFile([FromBody] AddFileDto addFile, CancellationToken token)
        {
            var filesInfo = await _service.AddFileAsync(addFile, token);
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