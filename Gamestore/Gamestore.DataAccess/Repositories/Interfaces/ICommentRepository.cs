using Gamestore.DataAccess.Entities;

namespace Gamestore.DataAccess.Repositories.Interfaces;

public interface ICommentRepository
{
    Task CreateAsync(Comment entity);

    Task UpdateAsync(Comment entity);

    Task<Comment?> GetByIdAsync(Guid id);

    Task<bool> DeleteAsync(Guid id);

    Task<bool> IsExistAsync(Guid id);

    Task<IEnumerable<Comment>?> GetByGameKeyAsync(string key);
}
