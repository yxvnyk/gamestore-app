using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Services.Interfaces;

namespace Gamestore.WebApi.Services;

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