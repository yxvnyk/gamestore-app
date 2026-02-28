using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.Domain.Models.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gamestore.DataAccess.Northwind.Context;

public class NorthwindDbContext
{
    private const string CategoriesCollectionName = "categories";
    private const string CustomersCollectionName = "customers";
    private const string EmployeeTerritoriesCollectionName = "employee-territories";
    private const string EmployeesCollectionName = "employees";
    private const string OrderDetailsCollectionName = "order-details";
    private const string OrdersCollectionName = "orders";
    private const string ProductsCollectionName = "products";
    private const string ShippersCollectionName = "shippers";
    private const string SuppliersCollectionName = "suppliers";
    private const string TerritoriesCollectionName = "territories";

    private readonly IMongoDatabase _database;

    public NorthwindDbContext(IOptions<NorthwindDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Category> Categories => _database.GetCollection<Category>(CategoriesCollectionName);

    public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>(CustomersCollectionName);

    public IMongoCollection<EmployeeTerritory> EmployeeTerritories => _database.GetCollection<EmployeeTerritory>(EmployeeTerritoriesCollectionName);

    public IMongoCollection<Employee> Employees => _database.GetCollection<Employee>(EmployeesCollectionName);

    public IMongoCollection<OrderDetails> OrderDetails => _database.GetCollection<OrderDetails>(OrderDetailsCollectionName);

    public IMongoCollection<Order> Orders => _database.GetCollection<Order>(OrdersCollectionName);

    public IMongoCollection<Product> Products => _database.GetCollection<Product>(ProductsCollectionName);

    public IMongoCollection<BsonDocument> Shippers => _database.GetCollection<BsonDocument>(ShippersCollectionName);

    public IMongoCollection<Supplier> Suppliers => _database.GetCollection<Supplier>(SuppliersCollectionName);

    public IMongoCollection<Territory> Territories => _database.GetCollection<Territory>(TerritoriesCollectionName);
}