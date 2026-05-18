namespace ECommerce.Catalog.Application.Products.Contracts;

public class ProductModel
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public required long CategoryId { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required int Quantity { get; set; }
}
