using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Helpers;
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
        return Ok(BanDurationHelper.GetSupportedDurations());
    }
}
