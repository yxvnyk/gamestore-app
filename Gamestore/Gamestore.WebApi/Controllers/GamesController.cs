using GameStore.Application.Helpers.Interfaces;
using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Helpers;
using Gamestore.Domain.Models.DTO.Comments;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController(IGameService gameService, IGenreService genreService,
    IPlatformService platformService, IPublisherService publisherService,
    IOrderItemService orderItemService,
    ICommentService commentService,
    IGenerateGameFile generateGameFile) : Controller
{
    private const string GameSuccessfullyUpdated = "Game successfuly updated";
    private static readonly Guid StubCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

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
    public async Task<IActionResult> GetAllGames([FromQuery] GetGamesRequest request)
    {
        var games = await gameService.GetAllGamesAsync(request);
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

    [HttpGet("{key}/comments")]
    public async Task<IActionResult> GetCommentsByGameKey(string key)
    {
        var publisher = await commentService.GetByGameKeyAsync(key);
        return Ok(publisher);
    }

    [HttpPost("{key}/buy")]
    public async Task<IActionResult> AddGameToCart(string key)
    {
        await orderItemService.AddGameToCartAsync(key, StubCustomerId);
        return Ok();
    }

    [HttpDelete("{key}/comments/{id:guid}")]
    public async Task<IActionResult> DeleteComment(string key, Guid id)
    {
        await commentService.DeleteAsync(key, id);
        return NoContent();
    }

    [HttpGet("pagination-options")]
    public IActionResult GetPaginationOptions()
    {
        return Ok(PaginationOptionsHelper.GetSupportedOptions());
    }

    [HttpGet("sorting-options")]
    public IActionResult GetSortingOptions()
    {
        return Ok(SortingOptionsHelper.GetSupportedOptions());
    }

    [HttpGet("publish-date-options")]
    public IActionResult GetPublishDateOptions()
    {
        return Ok(PublishDateFilterHelper.GetSupportedOptions());
    }
}
