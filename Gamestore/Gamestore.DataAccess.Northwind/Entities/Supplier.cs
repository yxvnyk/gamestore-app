using Gamestore.DataAccess.Northwind.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

[BsonIgnoreExtraElements]
public class Supplier
{
    [BsonId]
    public ObjectId InternalId { get; set; }

    [BsonElement("SupplierID")]
    public int SupplierId { get; set; }

    [BsonElement("CompanyName")]
    public string CompanyName { get; set; }

    [BsonElement("ContactName")]
    [BsonSerializer(typeof(StringSerializer))]
    public string ContactName { get; set; }

    [BsonElement("ContactTitle")]
    [BsonSerializer(typeof(StringSerializer))]
    public string ContactTitle { get; set; }

    [BsonElement("Address")]
    [BsonSerializer(typeof(StringSerializer))]
    public string Address { get; set; }

    [BsonElement("City")]
    [BsonSerializer(typeof(StringSerializer))]
    public string City { get; set; }

    [BsonElement("Region")]
    public string? Region { get; set; }

    [BsonElement("PostalCode")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? PostalCode { get; set; }

    [BsonElement("Country")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? Country { get; set; }

    [BsonElement("Phone")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? Phone { get; set; }

    [BsonElement("Fax")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? Fax { get; set; }

    [BsonElement("HomePage")]
    [BsonSerializer(typeof(StringSerializer))]
    public string? HomePage { get; set; }
}
