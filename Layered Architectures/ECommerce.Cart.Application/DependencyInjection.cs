using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICartService, CartService>();

        return services;
    }
}
