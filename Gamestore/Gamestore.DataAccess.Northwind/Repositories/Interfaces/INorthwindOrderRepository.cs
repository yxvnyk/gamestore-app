using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindOrderRepository
{
    Task<IEnumerable<Order>> GetHistoryAsync();
}
