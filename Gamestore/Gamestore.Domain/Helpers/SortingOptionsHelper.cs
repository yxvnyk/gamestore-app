using Gamestore.Domain.Enums;

namespace Gamestore.Domain.Helpers;

public static class SortingOptionsHelper
{
    private static readonly Dictionary<string, SortingOptions> Options = new()
    {
        { "Most popular", SortingOptions.MostPopular },
        { "Most commented", SortingOptions.MostCommented },
        { "Price ASC", SortingOptions.PriceAsc },
        { "Price DESC", SortingOptions.PriceDesc },
        { "New", SortingOptions.New },
    };

    public static SortingOptions? GetValue(string option)
    {
        Options.TryGetValue(option, out var result);
        return result;
    }

    public static IEnumerable<string> GetSupportedOptions()
    {
        return Options.Keys;
    }
}
