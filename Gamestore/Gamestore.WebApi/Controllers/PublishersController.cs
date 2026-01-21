using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishersController(IPublisherService publisherService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreatePublisher([FromBody] PublisherRequestDto publisher)
    {
        await publisherService.CreatePublisherAsync(publisher.Publisher);
        return Ok();
    }

    [HttpGet("{companyName:alpha}")]
    public async Task<IActionResult> GetPublisherByCompanyName(string companyName)
    {
        var publisher = await publisherService.GetPublisherByCompanyNameAsync(companyName);
        return Ok(publisher);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPublishers()
    {
        var publishers = await publisherService.GetAllPublishersAsync();
        return Ok(publishers);
    }
}
