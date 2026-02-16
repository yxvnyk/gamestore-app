using GameStore.Application.Helpers.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;

namespace Gamestore.Application.Helpers.Generators;

public class UniqueKeyGenerator(IGameRepository gameRepository) : IKeyGenerator
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<string> GenerateUniqueKeyAsync(string name)
    {
        var baseKey = ToKey(name);
        var key = baseKey;
        int suffix = 1;

        while (await _gameRepository.GameKeyExistAsync(key))
        {
            key = $"{baseKey}-{suffix}";
            suffix++;
        }

        return key;
    }

    private static string ToKey(string value)
    {
        return value
            .Trim()
            .ToLower(System.Globalization.CultureInfo.CurrentCulture)
            .Replace(" ", "-")
            .Replace(":", "-")
            .Replace("/", "-");
    }
}