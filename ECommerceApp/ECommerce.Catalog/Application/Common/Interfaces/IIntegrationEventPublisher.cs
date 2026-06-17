namespace ECommerce.Catalog.Application.Common.Interfaces;

public interface IIntegrationEventPublisher
{
    Task PublishIntegrationEventAsync<T>(
        T message,
        CancellationToken cancellationToken);
}
