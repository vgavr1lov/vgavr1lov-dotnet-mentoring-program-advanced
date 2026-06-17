namespace ECommerce.Cart.Infrastructure.Messaging;

public static class CartMessagingConstants
{
    public const string CatalogExchange = "catalog.events";
    public const string ProductUpdatedRoutingKey = "product.updated";
    public const string ProductUpdatedQueue = "cart.product.updated";
}
