using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Extension;

public static class NorthwindProductRepositoryExtension
{
    private const decimal MaxPrice = decimal.MaxValue;
    private const decimal LowestPossiblePrice = 0;

    public static IQueryable<Product> FilterByProductName(this IQueryable<Product> query, string? filterBy)
    {
        if (filterBy != null && filterBy.Length >= 3)
        {
            query = query.Where(g => g.ProductName.Contains(filterBy));
        }

        return query;
    }

    public static IQueryable<Product> FilterByPrice(this IQueryable<Product> query, double? minPrice, double? maxPrice)
    {
        decimal? maxPriceDecimal = (decimal?)maxPrice;
        decimal? minPriceDecimal = (decimal?)minPrice;
        if (maxPriceDecimal is null or < LowestPossiblePrice)
        {
            maxPriceDecimal = MaxPrice;
        }

        if (minPriceDecimal is null or < LowestPossiblePrice)
        {
            minPriceDecimal = LowestPossiblePrice;
        }

        return query.Where(g => g.UnitPrice >= minPriceDecimal && g.UnitPrice <= maxPriceDecimal);
    }

    public static IQueryable<Product> FilterByCategories(this IQueryable<Product> query, ICollection<string>? categoryIds)
    {
        if (categoryIds == null || categoryIds.Count == 0)
        {
            return query;
        }

        var parsedIds = ParseToIntCollection(categoryIds);

        if (parsedIds.Count == 0)
        {
            return query;
        }

        // return query
        return query.Where(g => parsedIds.Contains(g.CategoryId));
    }

    public static IQueryable<Product> FilterBySuppliers(this IQueryable<Product> query, ICollection<string>? supplierIds)
    {
        if (supplierIds == null || supplierIds.Count == 0)
        {
            return query;
        }

        var parsedIds = ParseToIntCollection(supplierIds);

        if (parsedIds.Count == 0)
        {
            return query;
        }

        // return query
        return query
            .Where(g => parsedIds.Contains(g.SupplierId));
    }

    private static ICollection<int> ParseToIntCollection(ICollection<string> ids)
    {
        var parsedIds = new List<int>();
        foreach (var raw in ids)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            if (int.TryParse(raw, out var id))
            {
                parsedIds.Add(id);
            }
        }

        return [.. parsedIds.Distinct()];
    }
}
