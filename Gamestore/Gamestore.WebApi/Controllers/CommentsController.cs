using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController() : Controller
{
    [HttpPost("ban")]
    public Task<IActionResult> BanUser()
    {
        throw new NotImplementedException();
    }

    [HttpGet("ban/duration")]
    public Task<IActionResult> GetGamesByGenre(Guid id)
    {
        throw new NotImplementedException();
    }
}
