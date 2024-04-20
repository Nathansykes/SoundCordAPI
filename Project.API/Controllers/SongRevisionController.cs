using Microsoft.AspNetCore.Mvc;
using Project.Domain.Songs;

namespace Project.API.Controllers;

[Route("api/")]
public class SongRevisionController(ISongRevisionService songRevisionService) : BaseController
{
    private readonly ISongRevisionService _songRevisionService = songRevisionService;

    [HttpGet("songs/{songId}/revisions")]
    public IActionResult GetRevisions(Guid songId)
    {
        var result = _songRevisionService.GetSongRevisions(songId);
        return Ok(result);
    }

    [HttpGet("revisions/{id}")]
    public IActionResult GetRevision(Guid id)
    {
        var result = _songRevisionService.GetSongRevisionById(id);
        return Ok(result);
    }

    [HttpPost("songs/{songId}/revisions")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> CreateRevision(Guid songId, CreateSongRevisionRequest model)
    {
        var result = await _songRevisionService.CreateSongRevision(songId, model);
        return Ok(result);
    }

}
