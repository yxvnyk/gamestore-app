using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlatformsController(IPlatformService platformService, IGameService gameService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreatePlatform([FromBody] PlatformDto platform)
    {
        if (platform == null)
        {
            return BadRequest("Platform data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await platformService.CreatePlatformAsync(platform);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetPlatformById(Guid id)
    {
        var platform = await platformService.GetPlatformByIdAsync(id);
        return Ok(platform);
    }

    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAllPlatfomrs()
    {
        var genres = await platformService.GetAllPlatformsAsync();
        return Ok(genres);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePlatform([FromBody] PlatformUpdateDto platform)
    {
        if (platform == null)
        {
            return BadRequest("Platform data is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await platformService.UpdatePlatformAsync(platform);
        return Ok($"Platform successfuly updated");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlatform(Guid id)
    {
        bool result = await platformService.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound($"Platform with ID {id} not found.");
    }

    [HttpGet("{id:guid}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGamesByPlatform(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Key is required.");
        }

        var game = await gameService.GetGamesByPlatformAsync(id);
        return Ok(game);
    }
}
