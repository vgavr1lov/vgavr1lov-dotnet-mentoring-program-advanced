using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Infrastructure.Data.Context;
using ECommerce.Catalog.Infrastructure.Data.Interfaces;
using ECommerce.Catalog.Infrastructure.Data.Outbox;
using ECommerce.Catalog.Infrastructure.Messaging;
using ECommerce.Messaging.RabbitMq;
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

        services.AddRabbitMq(config);
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddHostedService<CatalogMessagingInitializer>();
        services.AddHostedService<OutboxBackgroundProcessor>();

        return services;
    }
}
