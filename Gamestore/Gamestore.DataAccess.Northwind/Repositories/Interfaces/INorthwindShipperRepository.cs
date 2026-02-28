namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface INorthwindShipperRepository
{
    Task<IEnumerable<dynamic>> GetAllAsync();
}
