using ECommerce.Catalog.Domain.Entities;
using ECommerce.Common.Contracts.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace ECommerce.Catalog.Infrastructure.Data.Outbox;

public class OutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken)
    {
        InterceptProductUpdate(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void InterceptProductUpdate(DbContext? context)
    {
        if (context == null)
            return;

        var outboxMessages = new List<OutboxMessage>();

        var entities = context.ChangeTracker
            .Entries<Product>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entity in entities)
        {
            var productUpdatedIntegrationEvent = new ProductUpdatedIntegrationEvent
            {
                Id = entity.Entity.Id,
                Name = entity.Entity.Name,
                Description = entity.Entity.Description,
                ImageUrl = entity.Entity.Image?.Url,
                ImageAltText = entity.Entity.Image?.AltText,
                CategoryId = entity.Entity.CategoryId,
                Amount = entity.Entity.Price.Amount,
                Currency = entity.Entity.Price.Currency
            };

            outboxMessages.Add(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = typeof(ProductUpdatedIntegrationEvent).Name,
                Content = JsonSerializer.Serialize(productUpdatedIntegrationEvent),
                CreatedOn = DateTime.UtcNow
            });
        }

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
