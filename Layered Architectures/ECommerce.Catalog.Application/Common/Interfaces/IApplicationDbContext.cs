using ECommerce.Catalog.Domain.Entities;

namespace ECommerce.Catalog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Category { get; }
    DbSet<Product> Product { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
