namespace Gamestore.Domain.Models;

public class PagedList<T>
{
    public List<T> Items { get; set; }

    public int TotalCount { get; set; }
}
