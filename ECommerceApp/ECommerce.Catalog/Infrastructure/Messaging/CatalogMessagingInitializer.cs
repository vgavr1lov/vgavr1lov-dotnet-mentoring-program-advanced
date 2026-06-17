using ECommerce.Common.Abstractions.Messaging;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Catalog.Infrastructure.Messaging;

public class CatalogMessagingInitializer : IHostedService
{
    private readonly IMessageBusInitializer _initializer;

    public CatalogMessagingInitializer(IMessageBusInitializer initializer)
    {
        _initializer = initializer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _initializer.DeclareExchangeAsync(CatalogMessagingConstants.CatalogExchange, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
