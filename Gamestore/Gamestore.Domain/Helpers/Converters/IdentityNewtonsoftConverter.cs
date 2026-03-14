using Gamestore.Domain.Models;
using Newtonsoft.Json;

namespace Gamestore.Domain.Helpers.Converters;

public class IdentityNewtonsoftConverter : JsonConverter<Identity>
{
    public override Identity? ReadJson(JsonReader reader, Type objectType, Identity? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = reader.Value?.ToString();

        if (Guid.TryParse(value, out var guid))
        {
            return new Identity(guid, null);
        }

        if (int.TryParse(value, out var intId))
        {
            return new Identity(null, intId);
        }

        // exception
        throw new JsonException("Invalid ID format");
    }

    public override void WriteJson(JsonWriter writer, Identity? value, JsonSerializer serializer)
    {
        if (value.IsGuid)
        {
            writer.WriteValue(value.GuidId!.Value);
        }
        else if (value.IsInt)
        {
            writer.WriteValue(value.IntId!.Value);
        }
        else
        {
            writer.WriteNull();
        }
    }
}