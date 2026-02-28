using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class EmployeeTerritory
{
    [BsonElement("EmployeeID")]
    public int EmployeeId { get; set; }

    [BsonElement("TerritoryID")]
    public string TerritoryId { get; set; }
}
