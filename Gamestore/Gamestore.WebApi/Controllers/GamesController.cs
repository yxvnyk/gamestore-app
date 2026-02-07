using GameStore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Comments;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController(IGameService gameService, IGenreService genreService,
    IPlatformService platformService, IPublisherService publisherService,
    ICommentService commentService,
    IGenerateGameFile generateGameFile) : Controller
{
    private const string GameSuccessfullyUpdated = "Game successfuly updated";

    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest game)
    {
        await gameService.CreateGameAsync(game);
        return Ok();
    }

    [HttpGet("{key}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGameByKey(string key)
    {
        var game = await gameService.GetGameAsync(key);
        return Ok(game);
    }

    [HttpGet("find/{id:guid}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        var game = await gameService.GetGameAsync(id);
        return Ok(game);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await gameService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGame([FromBody] UpdateGameRequest game)
    {
        await gameService.UpdateGameAsync(game);
        return Ok(new { message = GameSuccessfullyUpdated });
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
        GameDto game = await gameService.GetGameAsync(key);
        var file = generateGameFile.GenerateFileDto(game);

        return File(file.Content, "text/plain", file.FileName);
    }

    [HttpGet("{key}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGenreByGameKey(string key)
    {
        var genres = await genreService.GetGenresByGameKeyAsync(key);
        return Ok(genres);
    }

    [HttpGet("{key}/platforms")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetPlatformsByGameKey(string key)
    {
        var platforms = await platformService.GetPlatformsByGameKeyAsync(key);
        return Ok(platforms);
    }

    [HttpGet("{key}/publisher")]
    public async Task<IActionResult> GetPublisherByCompanyName(string key)
    {
        var publisher = await publisherService.GetPublisherByGameKeyAsync(key);
        return Ok(publisher);
    }

    [HttpPost("{key}/comments")]
    public async Task<IActionResult> CreateComment(string key, [FromBody] CommentCreateDto comment)
    {
        await commentService.CreateAsync(comment, key);
        return Ok();
    }
}
