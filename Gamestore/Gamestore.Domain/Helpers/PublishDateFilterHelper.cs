using Gamestore.Domain.Enums;

namespace Gamestore.Domain.Helpers;

public static class PublishDateFilterHelper
{
    private static readonly Dictionary<string, PublishDateFilterOptions> SupportedOptions = new()
    {
        { "last week", PublishDateFilterOptions.LastWeek },
        { "last month", PublishDateFilterOptions.LastMonth },
        { "last year", PublishDateFilterOptions.LastYear },
        { "2 years", PublishDateFilterOptions.TwoYears },
        { "3 years", PublishDateFilterOptions.ThreeYears },
    };

    public static PublishDateFilterOptions GetValidFiltrationMethod(string? filterBy)
    {
        if (string.IsNullOrEmpty(filterBy))
        {
            return PublishDateFilterOptions.None;
        }

        // Normalize the input by trimming and converting to title case
        return SupportedOptions.TryGetValue(filterBy, out var result) ? result : PublishDateFilterOptions.None;
    }

    public static IEnumerable<string> GetSupportedOptions()
    {
        return SupportedOptions.Keys;
    }
}
