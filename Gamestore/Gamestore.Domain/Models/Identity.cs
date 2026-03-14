using Gamestore.Domain.Helpers.Converters;

namespace Gamestore.Domain.Models;

[Newtonsoft.Json.JsonConverter(typeof(IdentityNewtonsoftConverter))]
public record Identity(Guid? GuidId, int? IntId)
{
    public bool IsGuid => GuidId.HasValue;

    public bool IsInt => IntId.HasValue;
}
