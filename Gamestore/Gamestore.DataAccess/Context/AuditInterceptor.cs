using Gamestore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gamestore.DataAccess.Context;

public class AuditInterceptor(IAuditLogService auditLogService) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context == null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or
                        EntityState.Modified or
                        EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;
            var action = entry.State.ToString();

            Dictionary<string, object?>? oldValues = null;
            Dictionary<string, object?>? newValues = null;

            if (entry.State is EntityState.Modified)
            {
                newValues = entry.CurrentValues.Properties
                    .ToDictionary(p => p.Name, p => entry.CurrentValues[p]);
            }

            await auditLogService.LogChangeAsync(action, entityName, oldValues, newValues);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
