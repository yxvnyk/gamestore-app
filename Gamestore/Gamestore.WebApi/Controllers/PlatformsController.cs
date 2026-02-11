using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Platform;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PlatformsController(IPlatformService platformService, IGameService gameService) : Controller
{
    private const string PlatformSuccessfullyUpdated = "Platform successfuly updated";

    [HttpPost]
    public async Task<IActionResult> CreatePlatform([FromBody] CreatePlatformRequest platformRequest)
    {
        await platformService.CreatePlatformAsync(platformRequest.Platform);
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
    public async Task<IActionResult> UpdatePlatform([FromBody] UpdatePlatformRequest platformRequest)
    {
        await platformService.UpdatePlatformAsync(platformRequest.Platform);
        return Ok(new { message = PlatformSuccessfullyUpdated });
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
        var game = await gameService.GetGamesByPlatformAsync(id);
        return Ok(game);
    }
}
