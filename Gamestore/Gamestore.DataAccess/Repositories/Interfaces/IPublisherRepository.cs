using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface IPublisherRepository
{
    Task CreatePublisherAsync(Publisher entity);

    Task<bool> PublisherExistAsync(Guid id);
}
