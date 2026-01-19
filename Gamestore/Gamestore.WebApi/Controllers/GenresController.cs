using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController(IGenreService genreService, IGameService gameService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto genre)
    {
        await genreService.CreateGenreAsync(genre);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var genre = await genreService.GetGenreByIdAsync(id);
        return Ok(genre);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await genreService.GetAllGenresAsync();
        return Ok(genres);
    }

    [HttpGet("{id:guid}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGenreByParentId(Guid id)
    {
        var genres = await genreService.GetGenresByParentIdAsync(id);
        return Ok(genres);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGenre([FromBody] GenreUpdateDto genre)
    {
        await genreService.UpdateGenreAsync(genre);
        return Ok($"Genre successfuly updated");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        bool result = await genreService.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound($"Genre with ID {id} not found.");
    }

    [HttpGet("{id:guid}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGamesByGenre(Guid id)
    {
        var game = await gameService.GetGamesByGenreAsync(id);
        return Ok(game);
    }
}
