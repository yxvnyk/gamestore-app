using GameStore.Application.Helpers.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;

namespace GameStore.Application.Helpers;

public class UniqueKeyGenerator(IGameService gameRepository) : IKeyGenerator
{
    private readonly IGameService _gameRepository = gameRepository;

    public async Task<string> GenerateUniqueKeyAsync(string name)
    {
        var baseKey = ToKey(name);
        var key = baseKey;
        int suffix = 1;

        while (await _gameRepository.GameKeyExistAsync(key))
        {
            suffix++;
            key = $"{baseKey}-{suffix}";
        }

        return key;
    }

    public static string ToKey(string value)
    {
        return value
            .Trim()
            .ToLower(System.Globalization.CultureInfo.CurrentCulture)
            .Replace(" ", "-")
            .Replace(":", "-")
            .Replace("/", "-");
    }
}