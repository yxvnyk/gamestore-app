using Gamestore.Application.Services.Interfaces;
using Gamestore.Domain.Models.DTO.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Gamestore.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishersController(IPublisherService publisherService, IGameService gameService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> CreatePublisher([FromBody] CreatePublisherRequest publisher)
    {
        await publisherService.CreatePublisherAsync(publisher.Publisher);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePublisher([FromBody] UpdatePublisherRequest publisher)
    {
        await publisherService.UpdatePublisherAsync(publisher.Publisher);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        bool result = await publisherService.DeletePublisherAsync(id);
        return result ? NoContent() : NotFound($"Publisher with ID {id} not found.");
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

    [HttpGet("{companyName:alpha}/games")]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetGamesByPublisherName(string companyName)
    {
        var game = await gameService.GetGamesByCompanyNameAsync(companyName);
        return Ok(game);
    }
}
