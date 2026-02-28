using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Customer
{
    [BsonId]
    public int CustomerId { get; set; }

    [BsonElement("CompanyName")]
    public string CompanyName { get; set; }

    [BsonElement("ContactName")]
    public string ContactName { get; set; }

    [BsonElement("ContactTitle")]
    public string ContactTitle { get; set; }

    [BsonElement("Address")]
    public string Address { get; set; }

    [BsonElement("City")]
    public string City { get; set; }

    [BsonElement("Region")]
    public string? Region { get; set; }

    [BsonElement("PostalCode")]
    public string PostalCode { get; set; }

    [BsonElement("Country")]
    public string Country { get; set; }

    [BsonElement("Phone")]
    public string Phone { get; set; }

    [BsonElement("Fax")]
    public string? Fax { get; set; }
}
