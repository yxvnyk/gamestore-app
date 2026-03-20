using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Services.Interfaces;

public interface IPublisherService
{
    Task UpdatePublisherAsync(PublisherUpdateDto updateDto);

    Task<bool> DeletePublisherAsync(Identity identity);

    Task CreatePublisherAsync(PublisherCreateDto publisher);

    Task<PublisherDto?> GetPublisherByCompanyNameAsync(string companyName);

    Task<PublisherDto?> GetPublisherByGameKeyAsync(string key);

    Task<IEnumerable<PublisherDto>> GetAllPublishersAsync();
}
