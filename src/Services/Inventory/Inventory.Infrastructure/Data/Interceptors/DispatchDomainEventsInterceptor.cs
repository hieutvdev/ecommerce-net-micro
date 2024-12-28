using Inventory.Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Inventory.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator)
: SaveChangesInterceptor
{

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {

        await DispatchDomainEvents(eventData.Context);
        UpdateEntities(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    public async Task DispatchDomainEvents(DbContext? context)
    {
        if(context == null) return;

        var aggregates = context.ChangeTracker.Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);


        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();
        
        aggregates.ToList().ForEach(a => a.ClearDomainEvents());


        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
        
    }
    
    
    public void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IAggregate>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "admin";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = "admin";
                entry.Entity.LastModified = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.Entity.Deleted = true;
                entry.Entity.DeletedBy = "admin";
            }
        }
        
    }
    
}