namespace Gamestore.Domain.Interfaces;

public interface IUniqueKeyRepository
{
    Task<bool> GameKeyExistAsync(string key);
}
