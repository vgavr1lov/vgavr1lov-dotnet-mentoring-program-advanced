using ECommerce.Common.Abstractions.Messaging;
using ECommerce.Messaging.RabbitMq.Connection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ECommerce.Messaging.RabbitMq.Bus;

public class RabbitMqMessageBusConsumer<TCommand> : IMessageBusConsumer<TCommand>
    where TCommand : IRequest
{
    private const int ProcessingDelayInMilliseconds = 6000;
    private const int CircuitBreakerDelayInMinutes = 5;
    private const int NumberOfExceptions = 5;

    private readonly IRabbitMqConnectionManager _connectionManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker;

    public RabbitMqMessageBusConsumer(IRabbitMqConnectionManager connectionManager, IServiceProvider serviceProvider)
    {
        _connectionManager = connectionManager;
        _serviceProvider = serviceProvider;
        _circuitBreaker = ConfigurePolicy();
    }
    public async Task ConsumeAsync(string queue, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _circuitBreaker.ExecuteAsync(async () =>
                {
                    await RunConsumerAsync(queue, cancellationToken);
                });
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (BrokenCircuitException)
            {
                await Task.Delay(TimeSpan.FromMinutes(CircuitBreakerDelayInMinutes), cancellationToken);
                continue;
            }
            catch (Exception)
            {
                await Task.Delay(ProcessingDelayInMilliseconds, cancellationToken);
            }
        }
    }

    private async Task RunConsumerAsync(string queue, CancellationToken cancellationToken)
    {
        await using var channel = await _connectionManager.CreateChannelAsync();

        if (channel == null)
            throw new BrokerUnreachableException(new Exception("RabbitMQ connection is unavailable."));

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();

            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var command = JsonSerializer.Deserialize<TCommand>(message);

                if (command is null)
                    throw new ArgumentNullException();

                await sender.Send(command, cancellationToken);
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception)
            {
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer);

        var taskCompletionSource = new TaskCompletionSource();
        using var registration = cancellationToken.Register(() => taskCompletionSource.TrySetCanceled());
        await taskCompletionSource.Task;
    }

    private AsyncCircuitBreakerPolicy ConfigurePolicy()
    {
        return Policy
            .Handle<BrokerUnreachableException>()
            .Or<AlreadyClosedException>()
            .Or<SocketException>()
            .Or<TimeoutException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: NumberOfExceptions,
                durationOfBreak: TimeSpan.FromMinutes(CircuitBreakerDelayInMinutes));
    }
}