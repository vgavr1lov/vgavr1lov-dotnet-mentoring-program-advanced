namespace ECommerce.Cart.Infrastructure.Messaging;

public class CartMessagingConstants
{
    public const string CatalogExchange = "catalog.events";
    public const string ProductUpdatedRoutingKey = "product.updated";
    public const string ProductUpdatedQueue = "cart.product.updated";
}