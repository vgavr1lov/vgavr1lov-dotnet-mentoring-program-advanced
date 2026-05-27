namespace ECommerce.Common.Abstractions.Messaging;

public interface IMessageBusInitializer
{
    Task DeclareExchangeAsync(
    string exchange,
    CancellationToken cancellationToken);

    Task DeclareQueueAsync(
        string queue,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken);

    Task BindQueueAsync(
        string queue,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken);

    Task DeclareQueueWithDlqAsync(
        string queue,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken);
}