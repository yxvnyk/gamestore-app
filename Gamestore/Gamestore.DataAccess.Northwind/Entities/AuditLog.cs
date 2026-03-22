using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.DataAccess.Northwind.Entities;

[BsonIgnoreExtraElements]
public class AuditLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string ActionName { get; set; }

    public string EntityType { get; set; }

    public Dictionary<string, object?>? OldVersion { get; set; }

    public Dictionary<string, object?>? NewVersion { get; set; }
}
