namespace Gamestore.Application.Models;

public record Identity(Guid? GuidId, int? IntId)
{
    public bool IsGuid => GuidId.HasValue;

    public bool IsInt => IntId.HasValue;
}
