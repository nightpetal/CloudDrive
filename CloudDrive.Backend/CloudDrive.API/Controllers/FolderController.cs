using CloudDrive.API.Extensions;
using CloudDrive.Application.DTOs.FolderDTOs;
using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private readonly IFolderRepository _repo;
        private readonly IFolderService _service;

        public FolderController(IFolderRepository repo, IFolderService service)
        {
            _repo = repo;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(CancellationToken token, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var userId = User.GetUserId();
            return Ok(await _repo.GetAllAsync(userId, page, pageSize, token));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(Guid id, CancellationToken token)
        {
            var userId = User.GetUserId();
            var folder = await _repo.GetByIdAsync(userId, id, token);
            if (folder is null)
                return NotFound();
            return Ok(folder);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFolder([FromBody] AddFolderDto folderDto, CancellationToken token)
        {
            try
            {
                var userId = User.GetUserId();
                var folder = await _service.AddFolderAsync(userId, folderDto, token);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = folder.Id },
                    folder
                );
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFolder([FromBody] UpdateFolderDto folderDto, CancellationToken token)
        {
            try
            {
                var userId = User.GetUserId();
                var folder = await _service.UpdateFolderAsync(userId, folderDto, token);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteById([FromRoute] Guid id, CancellationToken token)
        {
            try
            {
                var userId = User.GetUserId();
                await _service.DeleteFolderAsync(userId, id, token);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}