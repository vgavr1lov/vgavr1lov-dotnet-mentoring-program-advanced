using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Domain.ValueObjects;
using ECommerce.Cart.Infrastructure.Messaging;
using ECommerce.Cart.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cart.WebAPI.Tests.IntegrationTests;

public class CustomWebAPIApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    private const long SampleCartId = 1;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(async services =>
        {
            RemoveService<ProductUpdatedMessageListener>(services);
            RemoveService<ICartRepository>(services);

            var dbPath = Path.Combine(
                Path.GetTempPath(),
                $"cart_test_{Guid.NewGuid():N}.db");

            if (File.Exists(dbPath))
                File.Delete(dbPath);

            services.AddScoped<ICartRepository>(x => new CartRepository(dbPath));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ICartRepository>();

            await SeedDatabaseAsync(repository);
        });

        builder.UseEnvironment("Development");
    }
    private static void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(T) || s.ImplementationType == typeof(T));

        if (descriptor != null)
            services.Remove(descriptor);
    }

    private static async Task SeedDatabaseAsync(ICartRepository repository)
    {
        var cart = new ShoppingCart(SampleCartId);

        var sampleCartItem = new CartItem(
            id: 1,
            name: "Product",
            image: new Image("https://example.com/product.jpg", "product"),
            price: new Money(10.20m, "PLN"),
            quantity: 1);

        cart.Items.Add(sampleCartItem);

        await repository.SaveCartAsync(cart, CancellationToken.None);
    }
}