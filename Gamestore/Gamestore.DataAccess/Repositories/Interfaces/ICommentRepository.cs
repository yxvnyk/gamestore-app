using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface ICommentRepository
{
    Task CreateAsync(Comment entity);

    Task UpdateAsync(Publisher entity);

    Task<bool> DeleteAsync(Guid id);

    Task<Comment?> GetByGameKeyAsync(string key);

    Task<bool> IsExistAsync(Guid id);
}
