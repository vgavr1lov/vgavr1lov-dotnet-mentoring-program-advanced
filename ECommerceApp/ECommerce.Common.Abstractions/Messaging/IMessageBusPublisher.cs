namespace ECommerce.Common.Abstractions.Messaging;

public interface IMessageBusPublisher
{
    Task PublishAsync<T>(
        string exchange,
        string routingKey,
        T message,
        CancellationToken cancellationToken = default);
}
