namespace ECommerce.Cart.Application.Carts.Contracts;

public class ItemModel
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public int Quantity { get; set; }
}
