using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController(ICommentService commentService) : Controller
{
    [HttpPost("ban")]
    public async Task<IActionResult> BanUser([FromBody] BanDto ban)
    {
        await commentService.BanUserByName(ban);
        return Ok();
    }

    [HttpGet("ban/durations")]
    public IActionResult GetBanDurationOptions()
    {
        return Ok(BanDurationOptions.GetSupportedDurations());
    }
}
