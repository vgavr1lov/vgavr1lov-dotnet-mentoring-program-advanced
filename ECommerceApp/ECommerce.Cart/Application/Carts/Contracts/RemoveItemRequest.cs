namespace ECommerce.Cart.Application.Carts.Contracts;

public class RemoveItemRequest
{
    public required long CartId { get; set; }
    public required long ItemId { get; set; }
}