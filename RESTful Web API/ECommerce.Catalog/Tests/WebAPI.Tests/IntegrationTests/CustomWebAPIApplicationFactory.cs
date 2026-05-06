using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Catalog.WebAPI.Tests.IntegrationTests;

public class CustomWebAPIApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            var dbName = $"CatalogWebAPITestsDb_{Guid.NewGuid():N}";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;Connect Timeout=30;Encrypt=False"));
        });

        builder.UseEnvironment("Development");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedDatabase(db);
        }

        return host;
    }

    private static void SeedDatabase(ApplicationDbContext db)
    {
        var sampleCategory = new Category(
            id: 1,
            name: "category",
            image: new Image("https://example.com/category.jpg", "category"),
            parentCategoryId: null);

        var sampleProduct = new Product(
            id: 1,
            name: "Product",
            description: "Product Description",
            image: new Image("https://example.com/product.jpg", "product"),
            categoryId: sampleCategory.Id,
            price: new Money(10.20m, "PLN"),
            amount: 1);

        db.Category.Add(sampleCategory);
        db.Product.Add(sampleProduct);
        db.SaveChanges();
    }
}