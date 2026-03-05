using Gamestore.Domain.Enums;
using Gamestore.Domain.Helpers;
using Gamestore.Domain.Models.DTO.Game;

namespace Gamestore.Application.Extensions;

public static class GameServiceExtension
{
    private const int AllItems = int.MaxValue;

    public static IEnumerable<GameDto> ApplySorting(this IEnumerable<GameDto> query, string? sortBy)
    {
        var sort = SortingOptionsHelper.GetValidSortingMethod(sortBy);
        return sort switch
        {
            SortType.MostPopular => throw new NotImplementedException(),
            SortType.MostCommented => query.OrderByDescending(x => x.CommentCount),
            SortType.PriceAsc => query.OrderBy(x => x.Price),
            SortType.PriceDesc => query.OrderByDescending(x => x.Price),
            SortType.New => query.OrderByDescending(x => x.CreatedDate),
            SortType.None => query,
            _ => query,
        };
    }

    public static IEnumerable<GameDto> ApplyPaging(this IEnumerable<GameDto> query, int pageNumber, int pageSize)
    {
        if (pageSize == AllItems)
        {
            return query;
        }

        var skip = (pageNumber - 1) * pageSize;
        return query.Skip(skip).Take(pageSize);
    }
}
