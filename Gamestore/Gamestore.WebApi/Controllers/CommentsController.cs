using Gamestore.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController() : Controller
{
    [HttpPost("ban")]
    public IActionResult BanUser()
    {
        throw new NotImplementedException();
    }

    [HttpGet("ban/duration")]
    public IActionResult GetBanDurationOptions()
    {
        return Ok(BanDurationOptions.GetSupportedDurations());
    }
}
