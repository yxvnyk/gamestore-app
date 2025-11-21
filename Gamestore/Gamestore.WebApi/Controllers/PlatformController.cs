using Gamestore.WebApi.Models.Models.DTO;
using Gamestore.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
public class PlatformController(IPlatformDatabaseService platformDatabaseService) : Controller
{
    [HttpPost("/platforms")]
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

        await platformDatabaseService.CreatePlatformAsync(platform);
        return Ok();
    }

    [HttpGet("/platforms/{id:guid}")]
    public async Task<IActionResult> GetPlatformById(Guid id)
    {
        var game = await platformDatabaseService.GetPlatformByIdAsync(id);
        return Ok(game);
    }

    [HttpGet("/platforms")]
    public async Task<IActionResult> GetAllPlatfomrs()
    {
        var genres = await platformDatabaseService.GetAllPlatformsAsync();
        return Ok(genres);
    }

    [HttpGet("/games/{key}/platform")]
    public async Task<IActionResult> GetPlatformыByGameKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return BadRequest("Key is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var game = await platformDatabaseService.GetPlatformsByGameKeyAsync(key);
        return Ok(game);
    }

    [HttpPut("/platform")]
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

        await platformDatabaseService.UpdatePlatformAsync(platform);
        return Ok($"Platform successfuly updated");
    }

    [HttpDelete("/platforms/{id:guid}")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        bool result = await platformDatabaseService.DeleteByIdAsync(id);
        return result ? NoContent() : NotFound($"Platform with ID {id} not found.");
    }
}
