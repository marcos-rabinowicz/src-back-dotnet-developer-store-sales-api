using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.ORM.DomainEvents;

internal static class DomainEventsHelper
{
    public static List<IDomainEvent> Collect(DbContext ctx)
    {
        return [.. ctx.ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvents>()
            .Where(e => e.DomainEvents.Count > 0)
            .SelectMany(e => e.DomainEvents)];
    }

    public static void Clear(DbContext ctx)
    {
        foreach (var entity in ctx.ChangeTracker.Entries()
                     .Select(e => e.Entity)
                     .OfType<IHasDomainEvents>()
                     .Where(e => e.DomainEvents.Count > 0))
        {
            entity.ClearDomainEvents();
        }
    }
}
