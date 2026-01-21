using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IPublisherRepository
{
    Task<Publisher?> GetPublisherByGameKeyAsync(string key);

    Task<Publisher?> GetPublisherByCompanyNameAsync(string companyName);

    Task<IEnumerable<Publisher>> GetAllPublishersAsync();

    Task CreatePublisherAsync(Publisher entity);

    Task<bool> PublisherExistAsync(Guid id);

    Task<bool> PublisherCompanyNameExistAsync(string companyName);
}
