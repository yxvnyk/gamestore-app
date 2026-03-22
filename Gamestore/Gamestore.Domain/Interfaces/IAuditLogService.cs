namespace Gamestore.Domain.Interfaces;

public interface IAuditLogService
{
    Task LogChangeAsync(string action, string entityType, Dictionary<string, object?>? oldValues, Dictionary<string, object?>? newValues);
}
