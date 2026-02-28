using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Regions
{
    [BsonId]
    public int RegionId { get; set; }

    [BsonElement("RegionDescription")]
    public string RegionDescription { get; set; }
}
