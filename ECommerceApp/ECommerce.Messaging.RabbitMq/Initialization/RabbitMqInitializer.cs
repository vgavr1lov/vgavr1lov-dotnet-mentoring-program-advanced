using ECommerce.Common.Abstractions.Messaging;
using ECommerce.Messaging.RabbitMq.Connection;
using RabbitMQ.Client;

namespace ECommerce.Messaging.RabbitMq.Initialization;

public class RabbitMqInitializer : IMessageBusInitializer
{
    private readonly IRabbitMqConnectionManager _connectionManager;

    public RabbitMqInitializer(IRabbitMqConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task DeclareExchangeAsync(
        string exchange,
        CancellationToken cancellationToken)
    {
        await using var channel = await _connectionManager.CreateChannelAsync();

        if (channel == null)
            return;

        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);
    }

    public async Task DeclareQueueAsync(
        string queue,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken)
    {
        await using var channel = await _connectionManager.CreateChannelAsync();

        if (channel == null)
            return;

        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            cancellationToken: cancellationToken);
    }

    public async Task BindQueueAsync(
        string queue,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken)
    {
        await using var channel = await _connectionManager.CreateChannelAsync();

        if (channel == null)
            return;

        await channel.QueueBindAsync(
            queue: queue,
            exchange: exchange,
            routingKey: routingKey,
            cancellationToken: cancellationToken);
    }

    public async Task DeclareQueueWithDlqAsync(
        string queue,
        string exchange,
        string routingKey,
        CancellationToken cancellationToken)
    {
        await using var channel = await _connectionManager.CreateChannelAsync();

        if (channel == null)
            return;

        var dlx = $"{exchange}.dlx";
        var dlq = $"{queue}.dead";
        var dlRoutingKey = $"{routingKey}.dead";

        await channel.ExchangeDeclareAsync(
            exchange: dlx,
            type: ExchangeType.Direct,
            durable: true);

        await channel.QueueDeclareAsync(
            queue: dlq,
            durable: true,
            exclusive: false,
            autoDelete: false);

        await channel.QueueBindAsync(
            queue: dlq,
            exchange: dlx,
            routingKey: dlRoutingKey);

        var args = new Dictionary<string, object?>
            {
                { "x-dead-letter-exchange",     dlx },
                { "x-dead-letter-routing-key",  dlRoutingKey }
            };

        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: args);
    }
}

