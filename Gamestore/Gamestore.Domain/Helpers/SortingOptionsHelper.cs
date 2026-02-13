using Gamestore.Domain.Enums;

namespace Gamestore.Domain.Helpers;

public static class SortingOptionsHelper
{
    private static readonly Dictionary<string, SortType> SupportedOptions = new()
    {
        { "Most popular", SortType.MostPopular },
        { "Most commented", SortType.MostCommented },
        { "Price ASC", SortType.PriceAsc },
        { "Price DESC", SortType.PriceDesc },
        { "New", SortType.New },
    };

    public static SortType GetValidSortingMethod(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
        {
            return SortType.None;
        }

        // Normalize the input by trimming and converting to title case
        return SupportedOptions.TryGetValue(sortBy, out var result) ? result : SortType.None;
    }

    public static IEnumerable<string> GetSupportedOptions()
    {
        return SupportedOptions.Keys;
    }
}
