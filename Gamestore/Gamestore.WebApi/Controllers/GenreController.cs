using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("")]
public class GenreController(IGenreDatabaseService genreDatabaseService) : Controller
{
    [HttpPost("/genres")]
    public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto genre)
    {
        if (genre == null)
        {
            return BadRequest("Genre data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await genreDatabaseService.CreateGenreAsync(genre);
        return Ok();
    }

    [HttpGet("/genres/{id:guid}")]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var game = await genreDatabaseService.GetGenreByIdAsync(id);
        return Ok(game);
    }

    [HttpGet("/genres")]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await genreDatabaseService.GetAllGenresAsync();
        return Ok(genres);
    }

    [HttpGet("/games/{key:alpha}/genres")]
    public async Task<IActionResult> GetGamesByGameKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var game = await genreDatabaseService.GetGenreByGameKeyAsync(key);
        return Ok(game);
    }

    [HttpGet("/genres/{id:guid}/genres")]
    public async Task<IActionResult> GetGenreByParentId(Guid id)
    {
        var genres = await genreDatabaseService.GetGenresByParentIdAsync(id);
        return Ok(genres);
    }

    [HttpPut("/genres")]
    public async Task<IActionResult> UpdateGenre([FromBody] GenreUpdateDto genre)
    {
        if (genre == null)
        {
            return BadRequest("Genre data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await genreDatabaseService.UpdateGenreAsync(genre);
        return Ok($"Game successfuly updated");
    }

    [HttpDelete("/genres/{id:guid}")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        bool result = await genreDatabaseService.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound($"Game with ID {id} not found.");
    }
}
