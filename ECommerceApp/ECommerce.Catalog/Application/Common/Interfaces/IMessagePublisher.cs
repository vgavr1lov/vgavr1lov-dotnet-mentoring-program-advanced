using ECommerce.Common.Contracts.Events;

namespace ECommerce.Catalog.Application.Common.Interfaces;

public interface IMessagePublisher
{
    Task PublishProductUpdatedIntegrationEventAsync(ProductUpdatedIntegrationEvent productUpdatedEvent, CancellationToken cancellationToken);
}
