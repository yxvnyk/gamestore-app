namespace Gamestore.DataAccess.Northwind.Repositories.Interfaces;

public interface IShipperRepository
{
    Task<IEnumerable<dynamic>> GetAllAsync();
}
