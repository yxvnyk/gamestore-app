using Gamestore.Domain.Interfaces;

namespace Gamestore.Domain.Generators;

public class UniqueKeyGenerator : IKeyGenerator
{
    public async Task<string> GenerateUniqueKeyAsync(IUniqueKeyRepository repository, string name)
    {
        var baseKey = ToKey(name);
        var key = baseKey;
        int suffix = 1;

        while (await repository.GameKeyExistAsync(key))
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