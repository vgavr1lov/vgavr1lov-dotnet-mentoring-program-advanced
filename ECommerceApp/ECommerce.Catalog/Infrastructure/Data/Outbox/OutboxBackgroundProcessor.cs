using System.Net.Sockets;
using System.Text.Json;
using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Infrastructure.Data.Interfaces;
using ECommerce.Common.Contracts.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.CircuitBreaker;

using RabbitMQ.Client.Exceptions;

namespace ECommerce.Catalog.Infrastructure.Data.Outbox;

public class OutboxBackgroundProcessor : BackgroundService
{
    private const int BatchSize = 20;
    private const int PauseDelayInMinutes = 10;
    private const int ProcessingDelayInMilliseconds = 6000;
    private const int CircuitBreakerDelayInMinutes = 5;
    private const int NumberOfExceptions = 5;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker;

    public OutboxBackgroundProcessor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        _circuitBreaker = ConfigurePolicy();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            var context = scope.ServiceProvider
                .GetRequiredService<IOutboxDbContext>();

            var messagePublisher = scope.ServiceProvider
                .GetRequiredService<IIntegrationEventPublisher>();

            var messages = await context.OutboxMessage
                .Where(x => x.NextRetryOn == null || x.NextRetryOn <= DateTime.Now)
                .OrderBy(x => x.CreatedOn)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            if (messages.Count == 0)
            {
                await Task.Delay(TimeSpan.FromMinutes(PauseDelayInMinutes), stoppingToken);
                continue;
            }

            foreach (var message in messages)
            {
                try
                {
                    await ProcessMessageAsync(message, messagePublisher, stoppingToken);
                    context.OutboxMessage.Remove(message);
                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (JsonException)
                {
                    message.RetryCount++;
                    message.NextRetryOn = DateTime.Now.AddMinutes(PauseDelayInMinutes * message.RetryCount);
                    await context.SaveChangesAsync(stoppingToken);
                    await Task.Delay(ProcessingDelayInMilliseconds, stoppingToken);
                    continue;
                }
                catch (BrokenCircuitException)
                {
                    await Task.Delay(TimeSpan.FromMinutes(CircuitBreakerDelayInMinutes), stoppingToken);
                    continue;
                }
                catch (Exception)
                {
                    await Task.Delay(ProcessingDelayInMilliseconds, stoppingToken);
                    continue;
                }
            }

            await Task.Delay(ProcessingDelayInMilliseconds, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(
        OutboxMessage message,
        IIntegrationEventPublisher messagePublisher,
        CancellationToken cancellationToken)
    {
        switch (message.Type)
        {
            case nameof(ProductUpdatedIntegrationEvent):
                {
                    var productUpdatedIntegrationEvent = JsonSerializer.Deserialize<ProductUpdatedIntegrationEvent>(message.Content);

                    if (productUpdatedIntegrationEvent is null)
                        return;

                    await _circuitBreaker.ExecuteAsync(async () => await messagePublisher.PublishIntegrationEventAsync(productUpdatedIntegrationEvent, cancellationToken));

                    break;
                }

            default:
                throw new InvalidOperationException($"Unknown outbox message type '{message.Type}' for message {message.Id}.");
        }
    }

    private AsyncCircuitBreakerPolicy ConfigurePolicy()
    {
        return Policy
            .Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .Or<TimeoutException>()
            .Or<AlreadyClosedException>()
            .Or<ArgumentNullException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: NumberOfExceptions,
                durationOfBreak: TimeSpan.FromMinutes(CircuitBreakerDelayInMinutes));
    }
}
