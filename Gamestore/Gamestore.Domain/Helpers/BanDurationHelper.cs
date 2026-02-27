namespace Gamestore.Domain.Helpers;

public static class BanDurationHelper
{
    private static readonly Dictionary<string, TimeSpan?> Options = new()
    {
        { "1 hour", TimeSpan.FromHours(1) },
        { "1 day", TimeSpan.FromDays(1) },
        { "1 week", TimeSpan.FromDays(7) },
        { "1 month", TimeSpan.FromDays(30) },
        { "permanent", null },
    };

    public static IEnumerable<string> GetSupportedDurations()
    {
        return Options.Keys;
    }

    public static bool IsSupported(string duration)
    {
        return Options.ContainsKey(duration);
    }
}
