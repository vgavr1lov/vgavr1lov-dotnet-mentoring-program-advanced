namespace ECommerce.Catalog.Application.Common.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishIntegrationEventAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken);
}
