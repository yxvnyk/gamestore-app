using System.Transactions;
using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Exceptions;

namespace Gamestore.Application.Services.Payments;

public class InventoryService(IGameRepository gameRepository,
    INorthwindProductRepository northwindProductRepository) : IInventoryService
{
    public async Task ReserveStockGameForOrderAsync(IEnumerable<OrderGame> items)
    {
        using var scope = CreateTransactionScope();

        foreach (var item in items)
        {
            var game = await gameRepository.GetByIdAsync(item.ProductId);

            if (game.UnitsInStock < item.Quantity)
            {
                throw new OrderLimitationException($"Not enough stock for '{game.Key}'. Required: {item.Quantity}, Available: {game.UnitsInStock}");
            }

            game.UnitsInStock -= item.Quantity;
            await gameRepository.UpdateGameAsync(game);
            await northwindProductRepository.SetUnitsInStockAsync(game.Key, item.Quantity);
        }

        scope.Complete();
    }

    public async Task RestoreStockGameForOrderAsync(IEnumerable<OrderGame> items)
    {
        using var scope = CreateTransactionScope();

        foreach (var item in items)
        {
            var game = await gameRepository.GetByIdAsync(item.ProductId);
            game.UnitsInStock += item.Quantity;
            await gameRepository.UpdateGameAsync(game);
            await northwindProductRepository.SetUnitsInStockAsync(game.Key, item.Quantity);
        }

        scope.Complete();
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}
