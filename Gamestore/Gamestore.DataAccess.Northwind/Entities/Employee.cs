using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Employee
{
    [BsonId]
    public int EmployeeId { get; set; }

    [BsonElement("LastName")]
    public string LastName { get; set; }

    [BsonElement("FirstName")]
    public string FirstName { get; set; }

    [BsonElement("Title")]
    public string Title { get; set; }

    [BsonElement("TitleOfCourtesy")]
    public string TitleOfCourtesy { get; set; }

    [BsonElement("BirthDate")]
    public DateTime? BirthDate { get; set; }

    [BsonElement("HireDate")]
    public DateTime? HireDate { get; set; }

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

    [BsonElement("HomePhone")]
    public string HomePhone { get; set; }

    [BsonElement("Extension")]
    public string Extension { get; set; }

    [BsonElement("Photo")]
    public byte[] Photo { get; set; }

    [BsonElement("Notes")]
    public string Notes { get; set; }

    [BsonElement("ReportsTo")]
    public int? ReportsTo { get; set; }

    [BsonElement("PhotoPath")]
    public string PhotoPath { get; set; }
}
