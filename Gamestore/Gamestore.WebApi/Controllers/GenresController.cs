using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Genre;
using Gamestore.WebApi.Helpers.Helpers.Binders;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController(IGenreService genreService, IGameService gameService) : Controller
{
    private const string GenreSuccessfullyUpdated = "Genre successfuly updated";

    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest genreRequest)
    {
        await genreService.CreateGenreAsync(genreRequest.Genre);
        return Ok();
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGenreById([ModelBinder(BinderType = typeof(IdentityModelBinder))] Identity id)
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

    [HttpGet("{id}/genres")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGenreByParentId([ModelBinder(BinderType = typeof(IdentityModelBinder))] Identity id)
    {
        var genres = await genreService.GetGenresByParentIdAsync(id);
        return Ok(genres);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGenre([FromBody] UpdateGenreRequest genreRequest)
    {
        await genreService.UpdateGenreAsync(genreRequest.Genre);
        return Ok(new { message = GenreSuccessfullyUpdated });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        bool result = await genreService.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound($"Genre with ID {id} not found.");
    }

    [HttpGet("{id}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGamesByGenre([ModelBinder(BinderType = typeof(IdentityModelBinder))] Identity id)
    {
        var game = await gameService.GetByGenreAsync(id);
        return Ok(game);
    }
}
