using Gamestore.DataAccess.Northwind.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Order
{
    [BsonId]
    public ObjectId InternalId { get; set; }

    [BsonElement("OrderID")]
    public int OrderId { get; set; }

    [BsonElement("CustomerID")]
    public string CustomerId { get; set; }

    [BsonElement("EmployeeID")]
    public int EmployeeId { get; set; }

    [BsonElement("OrderDate")]
    [BsonSerializer(typeof(SqlDateTimeSerializer))]
    public DateTime? OrderDate { get; set; }

    [BsonElement("RequiredDate")]
    [BsonSerializer(typeof(SqlDateTimeSerializer))]
    public DateTime? RequiredDate { get; set; }

    [BsonElement("ShippedDate")]
    [BsonSerializer(typeof(SqlDateTimeSerializer))]
    public DateTime? ShippedDate { get; set; }

    [BsonElement("ShipVia")]
    public int ShipVia { get; set; }

    [BsonElement("Freight")]
    public decimal Freight { get; set; }

    [BsonElement("ShipName")]
    public string ShipName { get; set; }

    [BsonElement("ShipAddress")]
    public string ShipAddress { get; set; }

    [BsonElement("ShipCity")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? ShipCity { get; set; }

    [BsonElement("ShipRegion")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? ShipRegion { get; set; }

    [BsonElement("ShipPostalCode")]
    [BsonSerializer(typeof(StringSerializer))]
    public string ShipPostalCode { get; set; }

    [BsonElement("ShipCountry")]
    [BsonSerializer(typeof(StringSerializer))]
    public string ShipCountry { get; set; }
}
