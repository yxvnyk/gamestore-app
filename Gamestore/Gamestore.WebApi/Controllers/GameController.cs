using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("games")]
public class GameController(IGameDatabaseService gameDatabaseService) : Controller
{
    [HttpPost("")]
    public async Task<IActionResult> CreateGame([FromBody] GameCreateExtendedDto game)
    {
        if (game == null)
        {
            return BadRequest("Game data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await gameDatabaseService.CreateGameAsync(game);
        return Ok();
    }

    [HttpGet("/{key:alpha}")]
    public async Task<IActionResult> GetGameByKey(string key)
    {
        if (key == null)
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return NotFound(ModelState);
        }

        var game = await gameDatabaseService.GetGameAsync(key);
        return Ok(game);
    }

    [HttpGet("/find/{id:guid}")]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var game = await gameDatabaseService.GetGameAsync(id);
        return Ok(game);
    }

    [HttpGet("/platforms/{id:guid}/games")]
    public async Task<IActionResult> GetGamesByPlatform(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var game = await gameDatabaseService.GetGamesByPlatformAsync(id);
        return Ok(game);
    }

    [HttpGet("/genres/{id:guid}/games")]
    public async Task<IActionResult> GetGamesByGenre(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return NotFound(ModelState);
        }

        var game = await gameDatabaseService.GetGamesByGenreAsync(id);
        return Ok(game);
    }

    [HttpPut("/games")]
    public async Task<IActionResult> UpdateGame([FromBody] GameUpdateExtendedDto game)
    {
        if (game == null)
        {
            return BadRequest("Game data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await gameDatabaseService.UpdateGameAsync(game);
        return !result ? NotFound("Game with the given ID does not exist.") : Ok("Game successfuly updated");
    }
}
