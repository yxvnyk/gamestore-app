using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Helpers;

namespace Gamestore.DataAccess.Extensions;

public static class GameRepositoryExtension
{
    private const int AllItems = int.MaxValue;

    public static IQueryable<Game> ApplySorting(this IQueryable<Game> query, string? sortBy)
    {
        var sort = SortingOptionsHelper.GetValidSortingMethod(sortBy);
        return sort switch
        {
            SortType.MostPopular => throw new NotImplementedException(),
            SortType.MostCommented => query.OrderByDescending(x => x.Comments.Count),
            SortType.PriceAsc => query.OrderBy(x => x.Price),
            SortType.PriceDesc => query.OrderByDescending(x => x.Price),
            SortType.New => query.OrderByDescending(x => x.CreatedDate),
            SortType.None => query,
            _ => query,
        };
    }

    public static IQueryable<Game> ApplyPaging(this IQueryable<Game> query, int pageNumber, int pageSize)
    {
        if (pageSize == AllItems)
        {
            return query;
        }

        var skip = (pageNumber - 1) * pageSize;
        return query.Skip(skip).Take(pageSize);
    }

    public static IQueryable<Game> ApplyPublishDateFiltration(this IQueryable<Game> query, string? filterBy)
    {
        var filter = PublishDateFilterHelper.GetValidFiltrationMethod(filterBy);

        DateTime? cutoffDate = filter switch
        {
            PublishDateFilterOptions.LastWeek => DateTime.UtcNow.AddDays(-7),
            PublishDateFilterOptions.LastMonth => DateTime.UtcNow.AddMonths(-1),
            PublishDateFilterOptions.LastYear => DateTime.UtcNow.AddYears(-1),
            PublishDateFilterOptions.TwoYears => DateTime.UtcNow.AddYears(-2),
            PublishDateFilterOptions.ThreeYears => DateTime.UtcNow.AddYears(-3),
            PublishDateFilterOptions.None => null,
            _ => null,
        };

        if (cutoffDate.HasValue)
        {
            query = query.Where(g => g.CreatedDate >= cutoffDate.Value);
        }

        return query;
    }
}
