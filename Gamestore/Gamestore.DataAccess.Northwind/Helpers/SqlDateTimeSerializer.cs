using System.Globalization;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Gamestore.DataAccess.Northwind.Helpers;

public class SqlDateTimeSerializer : SerializerBase<DateTime?>
{
    private readonly string _format = "yyyy-MM-dd HH:mm:ss.fff";

    public override DateTime? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        if (bsonType == MongoDB.Bson.BsonType.Null)
        {
            context.Reader.ReadNull();
            return null;
        }

        var dateStr = context.Reader.ReadString();
        if (DateTime.TryParseExact(dateStr, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }

        return DateTime.Parse(dateStr); // Fallback на стандартний парсинг
    }
}
