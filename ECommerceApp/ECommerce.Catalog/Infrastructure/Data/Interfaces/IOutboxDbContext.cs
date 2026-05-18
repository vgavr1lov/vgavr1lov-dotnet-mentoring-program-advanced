using ECommerce.Catalog.Infrastructure.Data.Outbox;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Infrastructure.Data.Interfaces;

public interface IOutboxDbContext
{
    DbSet<OutboxMessage> OutboxMessage { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}