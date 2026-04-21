using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class TestSqlServerFixture
{
    private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CatalogTestsDb;Integrated Security=True;Connect Timeout=30;Encrypt=False";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestSqlServerFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                _databaseInitialized = true;
            }
        }
    }

    public ApplicationDbContext CreateContext()
        => new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);
}
