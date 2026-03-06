using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

[BsonIgnoreExtraElements]
public class Category
{
    [BsonId]
    public ObjectId InternalId { get; set; }

    [BsonElement("CategoryID")]
    public int CategoryId { get; set; }

    [BsonElement("CategoryName")]
    public string Name { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("Picture")]
    public string Picture { get; set; }
}
