using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string dbPath)
    {
        services.AddScoped<ICartRepository>(x => new CartRepository(dbPath));

        return services;
    }
}
