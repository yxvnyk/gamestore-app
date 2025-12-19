using GameStore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController(IGameService gameService, IGenreService genreService,
    IPlatformService platformService, IGenerateGameFile generateGameFile) : Controller
{
    [HttpPost]
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

        await gameService.CreateGameAsync(game);
        return Ok();
    }

    [HttpGet("{key}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGameByKey(string key)
    {
        if (key == null)
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var game = await gameService.GetGameAsync(key);
        return Ok(game);
    }

    [HttpGet("find/{id:guid}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Key is required.");
        }

        var game = await gameService.GetGameAsync(id);
        return Ok(game);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAllGames()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var game = await gameService.GetAllGamesAsync();
        return Ok(game);
    }

    [HttpPut]
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

        await gameService.UpdateGameAsync(game);
        return Ok("Game successfuly updated");
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteGame(string key)
    {
        bool result = await gameService.DeleteByKeyAsync(key);
        return result ? NoContent() : NotFound($"Game with key {key} not found.");
    }

    [HttpGet("{key}/file")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGameFileByKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return BadRequest("Key is required.");
        }

        GameDto game = await gameService.GetGameAsync(key);
        var file = generateGameFile.GenerateFileDto(game);

        return File(file.Content, "text/plain", file.FileName);
    }

    [HttpGet("{key}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGenreByGameKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return BadRequest("Key is required.");
        }

        var genres = await genreService.GetGenresByGameKeyAsync(key);
        return Ok(genres);
    }

    [HttpGet("{key}/platform")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetPlatformыByGameKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return BadRequest("Key is required.");
        }

        var platforms = await platformService.GetPlatformsByGameKeyAsync(key);
        return Ok(platforms);
    }
}
