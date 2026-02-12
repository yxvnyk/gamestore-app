namespace Gamestore.Domain.Helpers;

public static class PaginationOptionsHelper
{
    private static readonly Dictionary<string, int> Options = new()
    {
        { "10", 10 },
        { "20", 20 },
        { "50", 50 },
        { "100", 100 },
        { "all", int.MinValue },
    };

    public static int GetValue(string option)
    {
        return Options.TryGetValue(option, out var value) ? value : 10;
    }

    public static IEnumerable<string> GetSupportedOptions()
    {
        return Options.Keys;
    }
}
