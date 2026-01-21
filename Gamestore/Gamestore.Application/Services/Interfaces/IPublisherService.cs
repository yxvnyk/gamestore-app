using Gamestore.Domain.Models.DTO.Publisher;

namespace Gamestore.Application.Services.Interfaces;

public interface IPublisherService
{
    Task CreatePublisherAsync(PublisherDto publisher);
}
