using Gamestore.Domain.Interfaces;

namespace Gamestore.Domain.Generators;

public interface IKeyGenerator
{
    Task<string> GenerateUniqueKeyAsync(IUniqueKeyRepository repository, string name);
}
