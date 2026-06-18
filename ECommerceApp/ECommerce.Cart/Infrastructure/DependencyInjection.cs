using ECommerce.Cart.Application.Carts.Commands.UpdateItem;
using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Infrastructure.Messaging;
using ECommerce.Cart.Infrastructure.Repositories;
using ECommerce.Common.Abstractions.Messaging;
using ECommerce.Messaging.RabbitMq;
using ECommerce.Messaging.RabbitMq.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var dbPath = config["LiteDb:DatabasePath"];
        services.AddScoped<ICartRepository>(x => new CartRepository(dbPath));

        services.AddRabbitMq(config);
        services.AddSingleton<IMessageBusConsumer<UpdateItemCommand>, RabbitMqMessageBusConsumer<UpdateItemCommand>>();
        services.AddHostedService<ProductUpdatedMessageListener>();

        return services;
    }
}
