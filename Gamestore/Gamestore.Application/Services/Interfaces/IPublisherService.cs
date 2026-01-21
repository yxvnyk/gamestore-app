using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Services.Interfaces;

public interface IPublisherService
{
    Task CreatePublisherAsync(PublisherDto publisher);

    Task<PublisherDto> GetPublisherByCompanyNameAsync(string companyName);

    Task<PublisherDto?> GetPublisherByGameKeyAsync(string key);

    Task<IEnumerable<PublisherDto>> GetAllPublishersAsync();
}
