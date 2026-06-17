using ECommerce.Cart.Application.Carts.Contracts;

namespace ECommerce.Cart.WebAPI.Tests.TestDataBuilders;

public class SampleCartItemBuilder
{
    private long _id = 1;
    private readonly string _name = "Product";
    private readonly string? _imageUrl = "https://example.com/product.jpg";
    private readonly string? _imageAltText = "Product";
    private readonly decimal _amount = 10.20m;
    private readonly string _currency = "PLN";
    private readonly int _quantity = 1;

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
        Quantity = _quantity,
    };
}
