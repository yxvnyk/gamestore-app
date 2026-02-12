using Gamestore.Domain.Enums;

namespace Gamestore.Domain.Helpers;

public static class PublishDateFilterHelper
{
    private static readonly Dictionary<string, PublishDateFilterOptions> Options = new()
    {
        { "last week", PublishDateFilterOptions.LastWeek },
        { "last month", PublishDateFilterOptions.LastMonth },
        { "last year", PublishDateFilterOptions.LastYear },
        { "2 years", PublishDateFilterOptions.TwoYears },
        { "3 years", PublishDateFilterOptions.ThreeYears },
    };

    public static PublishDateFilterOptions? GetValue(string option)
    {
        Options.TryGetValue(option, out var result);
        return result;
    }

    public static IEnumerable<string> GetSupportedOptions()
    {
        return Options.Keys;
    }
}
