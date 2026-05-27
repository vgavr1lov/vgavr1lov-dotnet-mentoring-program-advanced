using ECommerce.Common.Abstractions.Messaging;
using ECommerce.Messaging.RabbitMq.Connection;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace ECommerce.Messaging.RabbitMq.Bus;

public class RabbitMqMessageBusPublisher : IMessageBusPublisher
{
    private readonly IRabbitMqConnectionManager _connectionManager;

    public RabbitMqMessageBusPublisher(IRabbitMqConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task PublishAsync<T>(
        string exchange,
        string routingKey,
        T message,
        CancellationToken cancellationToken)
    {
        await using var channel = await _connectionManager.CreateChannelAsync();

        if (channel is null)
            throw new BrokerUnreachableException(new Exception("RabbitMQ connection is unavailable."));

        var serializedJsonMessage = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(serializedJsonMessage);

        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            body: body,
            cancellationToken: cancellationToken);
    }
}
