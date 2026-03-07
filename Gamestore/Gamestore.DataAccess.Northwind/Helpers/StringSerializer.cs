using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Gamestore.DataAccess.Northwind.Helpers;

public class StringSerializer : SerializerBase<string?>
{
    public override string? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();

        switch (bsonType)
        {
            case BsonType.String:
                return context.Reader.ReadString();
            case BsonType.Int32:
                return context.Reader.ReadInt32().ToString();
            case BsonType.Int64:
                return context.Reader.ReadInt64().ToString();
            case BsonType.Double:
                return context.Reader.ReadDouble().ToString();
            case BsonType.Null:
                context.Reader.ReadNull();
                return null;
            case BsonType.EndOfDocument:
                return null;

            case BsonType.Document:
                return null;
            case BsonType.Array:
                return null;
            case BsonType.Binary:
                return null;
            case BsonType.Undefined:
                return null;
            case BsonType.ObjectId:
                return null;
            case BsonType.Boolean:
                return null;
            case BsonType.DateTime:
                return null;
            case BsonType.RegularExpression:
                return null;
            case BsonType.JavaScript:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            case BsonType.Symbol:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            case BsonType.JavaScriptWithScope:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            case BsonType.Timestamp:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            case BsonType.Decimal128:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            case BsonType.MinKey:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            case BsonType.MaxKey:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
            default:
                throw new FormatException($"Cannot deserialize string from BsonType {bsonType}");
        }
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string? value)
    {
        if (value == null)
        {
            context.Writer.WriteNull();
        }
        else
        {
            context.Writer.WriteString(value);
        }
    }
}
