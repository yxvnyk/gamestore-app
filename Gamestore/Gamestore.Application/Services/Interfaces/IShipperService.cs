namespace Gamestore.Application.Services.Interfaces;

public interface IShipperService
{
    Task<IEnumerable<dynamic>> GetAllAsync();
}
