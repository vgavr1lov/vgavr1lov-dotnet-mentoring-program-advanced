using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Infrastructure.Data.Context;
using ECommerce.Catalog.Infrastructure.Data.Interfaces;
using ECommerce.Catalog.Infrastructure.Data.Outbox;
using ECommerce.Catalog.Infrastructure.Messaging;
using ECommerce.Common.Infrastructure.Messaging.Interfaces;
using ECommerce.Common.Infrastructure.Messaging.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("CatalogDb");

        services.AddScoped<OutboxSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            options.UseSqlServer(connectionString)
            .AddInterceptors(serviceProvider.GetRequiredService<OutboxSaveChangesInterceptor>()));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IOutboxDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        services.Configure<RabbitMqConfiguration>(
            config.GetSection("RabbitMqConfiguration"));

        services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();
        services.AddSingleton<IRabbitMqInitializer, RabbitMqInitializer>();
        services.AddSingleton<IMessageBus, RabbitMqMessageBus>();
        services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();

        services.AddHostedService<CatalogMessagingInitializer>();

        services.AddHostedService<OutboxBackgroundProcessor>();

        return services;
    }
}
