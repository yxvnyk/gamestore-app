namespace Gamestore.Domain.Helpers;

public static class PaginationOptionsHelper
{
    private static readonly Dictionary<string, int> Options = new()
    {
        { "10", 10 },
        { "20", 20 },
        { "50", 50 },
        { "100", 100 },
        { "all", int.MaxValue },
    };

    public static int CalculateTotalNumberOfPages(int totalItems, int pageSize)
    {
        if (pageSize <= 0)
        {
            return 1;
        }

        // Use ceiling to ensure that any remaining items are accounted for in an additional page
        return (int)Math.Ceiling((double)totalItems / pageSize);
    }

    public static int GetValidCurrentPage(int page)
    {
        return page < 1 ? 1 : page;
    }

    public static int GetValidPageSize(string? option)
    {
        if (option is null)
        {
            return 10;
        }

        // df
        return Options.TryGetValue(option, out var value) ? value : 10;
    }

    public static IEnumerable<string> GetSupportedOptions()
    {
        return Options.Keys;
    }
}
