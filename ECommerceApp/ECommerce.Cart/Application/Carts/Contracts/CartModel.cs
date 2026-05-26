namespace ECommerce.Cart.Application.Carts.Contracts;

public class CartModel
{
    public required long CartId { get; set; }

    public List<ItemModel>? Items { get; set; }
}
