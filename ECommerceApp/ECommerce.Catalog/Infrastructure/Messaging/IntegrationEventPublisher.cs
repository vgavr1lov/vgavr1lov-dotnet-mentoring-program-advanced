using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Common.Abstractions.Messaging;

namespace ECommerce.Catalog.Infrastructure.Messaging;

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IMessageBusPublisher _messageBus;

    public IntegrationEventPublisher(IMessageBusPublisher messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task PublishIntegrationEventAsync<T>(T integrationEvent, CancellationToken cancellationToken)
    {
        await _messageBus.PublishAsync(
            exchange: CatalogMessagingConstants.CatalogExchange,
            routingKey: CatalogMessagingConstants.ProductUpdatedRoutingKey,
            message: integrationEvent,
            cancellationToken: cancellationToken);
    }
}