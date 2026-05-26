using ECommerce.Common.Infrastructure.Messaging.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Catalog.Infrastructure.Messaging;

public class CatalogMessagingInitializer : IHostedService
{
    private readonly IRabbitMqInitializer _initializer;

    public CatalogMessagingInitializer(IRabbitMqInitializer initializer)
    {
        _initializer = initializer;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _initializer.DeclareExchangeAsync(CatalogMessagingConstants.CatalogExchange, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
