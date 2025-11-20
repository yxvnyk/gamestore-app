using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("")]
public class GenreController(IGenreDatabaseService genreDatabaseService) : Controller
{
    [HttpPost("/genres")]
    public async Task<IActionResult> CreateGenre([FromBody] GenreDto genre)
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
}
