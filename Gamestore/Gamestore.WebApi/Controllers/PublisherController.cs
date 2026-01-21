using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PublisherController(IPublisherService publisherService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreatePublisher([FromBody] PublisherRequestDto publisher)
    {
        await publisherService.CreatePublisherAsync(publisher.Publisher);
        return Ok();
    }
}
