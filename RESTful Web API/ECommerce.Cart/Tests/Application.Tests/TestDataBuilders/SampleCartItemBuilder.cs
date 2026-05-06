using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Domain.ValueObjects;

namespace ECommerce.Cart.Application.Tests.TestDataBuilders;

public class SampleCartItemBuilder
{
    private long _id = 1;
    private string _name = "Product";
    private Image? _image = new Image("https://example.com/product.jpg", "product");
    private Money _price = new Money(10.20m, "PLN");
    private int _quantity = 1;

    public SampleCartItemBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public CartItem Build() => new CartItem(_id, _name, _image, _price, _quantity);
}
