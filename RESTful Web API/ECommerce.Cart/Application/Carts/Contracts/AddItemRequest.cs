namespace ECommerce.Cart.Application.Carts.Contracts;

public class AddItemRequest
{
    public required long CartId { get; set; }
    public required long ItemId { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public int Quantity { get; set; }

}
