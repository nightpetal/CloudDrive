using CloudDrive.API.Extensions;
using CloudDrive.Application.DTOs.Request;
using CloudDrive.Application.DTOs.Response;
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
        public async Task<ActionResult<PagedResult<FilesInfo>>> GetFiles(CancellationToken token, [FromQuery] Guid? folderId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var userId = User.GetUserId();

            var files = await _repo.GetAllAsync(userId, folderId, page, pageSize, token);

            var dto = new PagedResult<FileRequest>
            {
                Data = files.Data
                .Select(file => file.MapFile())
                .ToList(),
                Page = files.Page,
                PageSize = files.PageSize,
                HasNextPage = files.HasNextPage
            };

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

        [HttpGet("download/{id:guid}")]
        public async Task<IActionResult> DownloadFile(Guid id, CancellationToken token)
        {
            try
            {
                var userId = User.GetUserId();
                var file = await _repo.GetByIdAsync(userId, id, token);

                if (file == null)
                    return NotFound(new { message = "File not found" });

                var fileStream = await _service.DownloadFileAsync(file.StorageKey, token);

                return File(fileStream, file.MimeType, file.OrginalName);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<FilesInfo>> AddFile([FromBody] AddFileDto addFile, CancellationToken token)
        {
            var userId = User.GetUserId();
            var filesInfo = await _service.AddFileAsync(userId, addFile, token);
            return CreatedAtAction(
                nameof(GetFiles),
                new { id = filesInfo.Id },
                filesInfo
            );
        }

        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<FilesInfo>> UploadFile(CancellationToken token, [FromQuery] Guid? folderId = null)
        {
            var userId = User.GetUserId();

            // Check if file is present in the request
            if (Request.Form.Files.Count == 0)
                return BadRequest("No file provided");

            var file = Request.Form.Files[0];

            if (file.Length == 0)
                return BadRequest("File is empty");

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadedFile = await _service.UploadFileAsync(
                        userId,
                        file.FileName,
                        stream,
                        file.ContentType,
                        folderId,
                        token
                    );

                    return CreatedAtAction(nameof(GetFileById), new { id = uploadedFile.Id }, uploadedFile);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FilesInfo>> DeleteFile([FromRoute] Guid id, CancellationToken token)
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