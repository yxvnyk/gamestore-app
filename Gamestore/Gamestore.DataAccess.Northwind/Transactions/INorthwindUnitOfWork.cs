using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using MongoDB.Driver;

namespace Gamestore.DataAccess.Northwind.Transactions;

public interface INorthwindUnitOfWork
{
    INorthwindProductRepository Products { get; }

    INorthwindSupplierRepository Suppliers { get; }

    INorthwindCategoryRepository Categories { get; }

    Task<IClientSessionHandle> StartTransactionAsync();

    Task CommitTransactionAsync();

    Task AbortTransactionAsync();
}
