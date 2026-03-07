using Gamestore.DataAccess.Northwind.Entities;

namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindOrderDetailsRepository
{
    Task<IEnumerable<OrderDetails>> GetByOrderIdAsync(int id);
}
