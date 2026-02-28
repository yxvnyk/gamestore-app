using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Product
{
    [BsonId]
    public int ProductId { get; set; }

    [BsonElement("ProductName")]
    public string ProductName { get; set; }

    [BsonElement("SupplierID")]
    public int SupplierId { get; set; }

    [BsonElement("CategoryID")]
    public int CategoryId { get; set; }

    [BsonElement("QuantityPerUnit")]
    public string QuantityPerUnit { get; set; }

    [BsonElement("UnitPrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal UnitPrice { get; set; }

    [BsonElement("UnitsInStock")]
    public int UnitsInStock { get; set; }

    [BsonElement("UnitsOnOrder")]
    public int UnitsOnOrder { get; set; }

    [BsonElement("ReorderLevel")]
    public int ReorderLevel { get; set; }

    [BsonElement("Discontinued")]
    public bool Discontinued { get; set; }
}
