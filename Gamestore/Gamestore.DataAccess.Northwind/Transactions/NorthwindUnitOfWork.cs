using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Repositories;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Driver;

namespace Gamestore.DataAccess.Northwind.Transactions;

public class NorthwindUnitOfWork(NorthwindDbContext context, IMongoClient client) : INorthwindUnitOfWork
{
    private readonly IMongoClient _client = client;

    private IClientSessionHandle _session;

    public INorthwindProductRepository Products { get; } = new NorthwindProductRepository(context);

    public INorthwindSupplierRepository Suppliers { get; } = new NorthwindSupplierRepository(context);

    public INorthwindCategoryRepository Categories { get; } = new NorthwindCategoryRepository(context);

    public async Task AbortTransactionAsync()
    {
        if (_session != null && _session.IsInTransaction)
        {
            await _session.AbortTransactionAsync();
        }
    }

    public async Task CommitTransactionAsync()
    {
        if (_session != null && _session.IsInTransaction)
        {
            await _session.CommitTransactionAsync();
        }
    }

    public async Task<IClientSessionHandle> StartTransactionAsync()
    {
        _session = await _client.StartSessionAsync();
        _session.StartTransaction();
        return _session;
    }
}
