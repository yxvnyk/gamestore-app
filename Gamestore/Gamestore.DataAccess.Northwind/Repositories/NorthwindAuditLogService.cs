using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Entities;
using Gamestore.Domain.Interfaces;

namespace Gamestore.DataAccess.Northwind.Repositories;

public class NorthwindAuditLogService(NorthwindDbContext context) : IAuditLogService
{
    private readonly NorthwindDbContext _context = context;

    public async Task LogChangeAsync(string action, string entityType, Dictionary<string, object?>? oldValues, Dictionary<string, object?>? newValues)
    {
        var logEntry = new AuditLog
        {
            Timestamp = DateTime.UtcNow,
            ActionName = action,
            EntityType = entityType,
            OldVersion = oldValues,
            NewVersion = newValues,
        };

        await _context.AuditLog.InsertOneAsync(logEntry);
    }
}
