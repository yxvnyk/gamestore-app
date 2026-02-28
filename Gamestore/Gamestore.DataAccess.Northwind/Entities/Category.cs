using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

public class Category
{
    [BsonId]
    public int CategoryId { get; set; }

    [BsonElement("CategoryName")]
    public string Name { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }

    [BsonElement("Picture")]
    public byte[] Picture { get; set; }
}
