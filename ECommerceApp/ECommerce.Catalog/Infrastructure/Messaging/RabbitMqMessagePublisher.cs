using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Common.Contracts.Events;
using ECommerce.Common.Infrastructure.Messaging.Interfaces;

namespace ECommerce.Catalog.Infrastructure.Messaging;

public class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly IMessageBus _messageBus;

    public RabbitMqMessagePublisher(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }
    public async Task PublishProductUpdatedIntegrationEventAsync(ProductUpdatedIntegrationEvent productUpdatedEvent, CancellationToken cancellationToken)
    {
        await _messageBus.PublishAsync(
            exchange: CatalogMessagingConstants.CatalogExchange,
            routingKey: CatalogMessagingConstants.ProductUpdatedRoutingKey,
            message: productUpdatedEvent,
            cancellationToken: cancellationToken);
    }
}
