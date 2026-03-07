using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class OrderDetails
{
    [BsonId]
    public ObjectId InternalId { get; set; }

    [BsonElement("OrderID")]
    public int OrderId { get; set; }

    [BsonElement("ProductID")]
    public int ProductId { get; set; }

    [BsonElement("UnitPrice")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal UnitPrice { get; set; }

    [BsonElement("Quantity")]
    public short Quantity { get; set; }

    [BsonElement("Discount")]
    public float Discount { get; set; }
}
