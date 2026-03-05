using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Wrappers;
using Gamestore.Domain.Enums;
using Gamestore.Domain.Helpers;

namespace Gamestore.DataAccess.Extensions;

public static class GameRepositoryExtension
{
    private const int AllItems = int.MaxValue;
    private const double MaxPrice = double.MaxValue;
    private const double LowestPossiblePrice = 0;

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

    public static IQueryable<GameWithStats> MapToGameWithStats(this IQueryable<Game> query)
    {
        var prejectedQuery = query.Select(g => new GameWithStats
        {
            Game = g,
            CommentCount = g.Comments.Count,
            CreatedDate = g.CreatedDate,
        });
        return prejectedQuery;
    }

    public static IQueryable<Game> FilterByPublishDate(this IQueryable<Game> query, string? filterBy)
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

    public static IQueryable<Game> FilterByGameName(this IQueryable<Game> query, string? filterBy)
    {
        if (filterBy != null && filterBy.Length >= 3)
        {
            query = query.Where(g => g.Name.Contains(filterBy));
        }

        return query;
    }

    public static IQueryable<Game> FilterByPrice(this IQueryable<Game> query, double? minPrice, double? maxPrice)
    {
        if (maxPrice is null or < LowestPossiblePrice)
        {
            maxPrice = MaxPrice;
        }

        if (minPrice is null or < LowestPossiblePrice)
        {
            minPrice = LowestPossiblePrice;
        }

        return query.Where(g => g.Price >= minPrice && g.Price <= maxPrice);
    }

    public static IQueryable<Game> FilterByGenres(this IQueryable<Game> query, ICollection<Guid>? genreIds)
    {
        if (genreIds == null || genreIds.Count == 0)
        {
            return query;
        }

        // return filtered query
        return query
            .Where(g => g.GameGenres
            .Any(gg => genreIds
                .Contains(gg.GenreId)));
    }

    public static IQueryable<Game> FilterByPlatforms(this IQueryable<Game> query, ICollection<Guid>? platformsIds)
    {
        if (platformsIds == null || platformsIds.Count == 0)
        {
            return query;
        }

        // return filtered query
        return query
            .Where(g => g.GamePlatforms
            .Any(gg => platformsIds
                .Contains(gg.PlatformId)));
    }

    public static IQueryable<Game> FilterByPublishers(this IQueryable<Game> query, ICollection<Guid>? publisherIds)
    {
        if (publisherIds == null || publisherIds.Count == 0)
        {
            return query;
        }

        // return filtered query
        return query
            .Where(g => publisherIds
                .Contains(g.PublisherId));
    }
}
