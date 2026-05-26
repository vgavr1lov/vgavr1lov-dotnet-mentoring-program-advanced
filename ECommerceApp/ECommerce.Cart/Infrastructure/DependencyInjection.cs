using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Infrastructure.Messaging;
using ECommerce.Cart.Infrastructure.Repositories;
using ECommerce.Common.Infrastructure.Messaging.Interfaces;
using ECommerce.Common.Infrastructure.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var dbPath = config["LiteDb:DatabasePath"];
        services.AddScoped<ICartRepository>(x => new CartRepository(dbPath));

        services.Configure<RabbitMqConfiguration>(
            config.GetSection("RabbitMqConfiguration"));

        services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();
        services.AddSingleton<IRabbitMqInitializer, RabbitMqInitializer>();
        services.AddHostedService<ProductUpdatedMessageListener>();

        return services;
    }
}
