using ECommerce.Cart.Application.Carts.Contracts;

namespace ECommerce.Cart.WebAPI.Tests.TestDataBuilders;

public class SampleCartItemBuilder
{
    private long _id = 1;
    private string _name = "Product";
    private string? _imageUrl = "https://example.com/product.jpg";
    private string? _imageAltText = "Product";
    private decimal _amount = 10.20m;
    private string _currency = "PLN";
    private int _quantity = 1;

    public SampleCartItemBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public ItemModel Build() => new ItemModel
    {
        Id = _id,
        Name = _name,
        ImageUrl = _imageUrl,
        ImageAltText = _imageAltText,
        Amount = _amount,
        Currency = _currency,
        Quantity = _quantity
    };
}
