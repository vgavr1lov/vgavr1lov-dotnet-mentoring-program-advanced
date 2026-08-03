using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Domain.ValueObjects;

namespace ECommerce.Cart.Application.Tests.TestDataBuilders;

public class SampleCartItemBuilder
{
    private long _id = 1;
    private readonly string _name = "Product";
    private readonly Image? _image = new Image("https://example.com/product.jpg", "product");
    private readonly Money _price = new Money(10.20m, "PLN");
    private readonly int _quantity = 1;

    public SampleCartItemBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public CartItem Build() => new CartItem(_id, _name, _image, _price, _quantity);
}
