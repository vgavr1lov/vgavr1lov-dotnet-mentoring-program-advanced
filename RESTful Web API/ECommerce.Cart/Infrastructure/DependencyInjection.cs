using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var dbPath = config["LiteDb:DatabasePath"];
        services.AddScoped<ICartRepository>(x => new CartRepository(dbPath));

        return services;
    }
}
