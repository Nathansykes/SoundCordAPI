using Microsoft.AspNetCore.Mvc;
using Project.Domain.Songs;

namespace Project.API.Controllers;
[Route("api")]
public class SongsController(ISongService songService) : BaseController
{
    private readonly ISongService _songService = songService;

    [HttpGet("groups/{groupId}/songs")]
    public IActionResult GetSongs(Guid groupId)
    {
        var result = _songService.GetAllSongsForGroup(groupId);
        return Ok(result);
    }

    [HttpGet("songs/{id}")]
    public IActionResult GetSong(Guid id)
    {
        var result = _songService.GetSongById(id);
        return Ok(result);
    }

    [HttpPost("groups/{groupId}/songs")]
    public IActionResult CreateSong(Guid groupId, SongModel model)
    {
        var result = _songService.CreateSong(groupId, model);
        return Ok(result);
    }
}
