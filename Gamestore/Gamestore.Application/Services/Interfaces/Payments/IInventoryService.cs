using Gamestore.DataAccess.Entities;

namespace Gamestore.Application.Services.Interfaces.Payments;

public interface IInventoryService
{
    Task ReserveStockGameForOrderAsync(IEnumerable<OrderGame> items);

    Task RestoreStockGameForOrderAsync(IEnumerable<OrderGame> items);
}
