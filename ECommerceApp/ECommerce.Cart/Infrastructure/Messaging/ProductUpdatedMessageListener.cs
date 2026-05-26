using ECommerce.Cart.Application.Carts.Commands.UpdateItem;
using ECommerce.Common.Infrastructure.Messaging.Interfaces;
using ECommerce.Common.Infrastructure.Messaging.RabbitMq;
using MediatR;

namespace ECommerce.Cart.Infrastructure.Messaging;

public class ProductUpdatedMessageListener : RabbitMqListenerBase<UpdateItemCommand>
{
    protected override string Queue => CartMessagingConstants.ProductUpdatedQueue;
    private readonly IRabbitMqInitializer _initializer;

    public ProductUpdatedMessageListener(
        IRabbitMqConnectionManager connectionManager,
        IServiceProvider serviceProvider,
        IRabbitMqInitializer initializer,
        ISender sender) : base(connectionManager, serviceProvider)
    {
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

        await base.ExecuteAsync(cancellationToken);
    }
}
