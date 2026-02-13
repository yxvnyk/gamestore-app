using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Helpers;

namespace Gamestore.DataAccess.Extensions;

public static class GameRepositoryExtension
{
    private const int AllItems = int.MaxValue;

    public static IQueryable<Game> SortingOrDefault(this IQueryable<Game> query, string? sortBy)
    {
        var sort = SortingOptionsHelper.GetValidSortingMethod(sortBy);
        return sort switch
        {
            SortType.MostPopular => throw new NotImplementedException(),
            SortType.MostCommented => query.OrderByDescending(x => x.Comments.Count),
            SortType.PriceAsc => query.OrderBy(x => x.Price),
            SortType.PriceDesc => query.OrderByDescending(x => x.Price),
            SortType.New => throw new NotImplementedException(),
            SortType.None => query,
            _ => query,
        };
    }

    public static IQueryable<Game> Paging(this IQueryable<Game> query, int pageNumber, int pageSize)
    {
        if (pageSize == AllItems)
        {
            return query;
        }

        var skip = (pageNumber - 1) * pageSize;
        return query.Skip(skip).Take(pageSize);
    }
}
