using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Territory
{
    [BsonElement("TerritoryID")]
    public string TerritoryId { get; set; }

    [BsonElement("TerritoryDescription")]
    public string TerritoryDescription { get; set; }

    [BsonElement("RegionID")]
    public int RegionId { get; set; }
}
