using ECommerce.Cart.Application.Carts.Commands.UpdateItem;
using ECommerce.Common.Abstractions.Messaging;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Cart.Infrastructure.Messaging;

public class ProductUpdatedMessageListener : BackgroundService
{
    private readonly IMessageBusConsumer<UpdateItemCommand> _consumer;
    private readonly IMessageBusInitializer _initializer;

    public ProductUpdatedMessageListener(
        IMessageBusConsumer<UpdateItemCommand> consumer,
        IMessageBusInitializer initializer)
    {
        _consumer = consumer;
        _initializer = initializer;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await _initializer.DeclareExchangeAsync(CartMessagingConstants.CatalogExchange, cancellationToken);
        await _initializer.DeclareQueueWithDlqAsync(
            CartMessagingConstants.ProductUpdatedQueue,
            CartMessagingConstants.CatalogExchange,
            CartMessagingConstants.ProductUpdatedRoutingKey,
            cancellationToken);
        await _initializer.BindQueueAsync(
            CartMessagingConstants.ProductUpdatedQueue,
            CartMessagingConstants.CatalogExchange,
            CartMessagingConstants.ProductUpdatedRoutingKey,
            cancellationToken);

        await _consumer.ConsumeAsync(CartMessagingConstants.ProductUpdatedQueue, cancellationToken);
    }
}
