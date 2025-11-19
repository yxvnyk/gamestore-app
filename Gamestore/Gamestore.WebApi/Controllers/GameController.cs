using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("games")]
public class GameController(IGameDatabaseService gameDatabaseService) : Controller
{
    [HttpPost("")]
    public async Task<IActionResult> CreateGame([FromBody] GameCreateDto game)
    {
        if (game == null)
        {
            return BadRequest("Game data is required.");
        }

        if (!ModelState.IsValid)
        {
            return NotFound(ModelState);
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
}
