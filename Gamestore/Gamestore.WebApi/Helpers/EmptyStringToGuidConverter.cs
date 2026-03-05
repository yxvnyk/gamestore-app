using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gamestore.WebApi.Helpers;

public class EmptyStringToGuidConverter : JsonConverter<Guid?>
{
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            return string.IsNullOrEmpty(stringValue) ? null : Guid.Parse(stringValue);
        }

        throw new JsonException("Expected a string value.");
    }

    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString());
        }
        else
        {
            writer.WriteStringValue(string.Empty);
        }
    }
}
