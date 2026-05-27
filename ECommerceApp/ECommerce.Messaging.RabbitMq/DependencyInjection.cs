using ECommerce.Common.Abstractions.Messaging;
using ECommerce.Messaging.RabbitMq.Bus;
using ECommerce.Messaging.RabbitMq.Configuration;
using ECommerce.Messaging.RabbitMq.Connection;
using ECommerce.Messaging.RabbitMq.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Messaging.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<RabbitMqConfiguration>(config.GetSection("RabbitMqConfiguration"));
        services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();
        services.AddSingleton<IMessageBusPublisher, RabbitMqMessageBusPublisher>();
        services.AddSingleton<IMessageBusInitializer, RabbitMqInitializer>();

        return services;
    }
}
