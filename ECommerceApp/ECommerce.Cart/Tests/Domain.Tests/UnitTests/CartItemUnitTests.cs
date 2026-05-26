using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Domain.ValueObjects;

namespace ECommerce.Cart.Domain.Tests.UnitTests;

public class CartItemUnitTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenIdIsNotProvided()
    {
        Assert.Throws<ArgumentException>(() => new CartItem(
            id: 0,
            name: "item1",
            image: new Image("https://example.com/product.jpg", "product"),
            price: new Money(10.20m, "PLN"),
            quantity: 1));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsBlank()
    {
        Assert.Throws<ArgumentException>(() => new CartItem(
            id: 1,
            name: "",
            image: new Image("https://example.com/product.jpg", "product"),
            price: new Money(10.20m, "PLN"),
            quantity: 1));
    }

    [Fact]
    public void Constructor_ShouldCreate_WhenIdAndNameAreProvided()
    {
        var cartItem = new CartItem(
            id: 1,
            name: "item1",
            image: new Image("https://example.com/product.jpg", "product"),
            price: new Money(10.20m, "PLN"),
            quantity: 1);

        Assert.Equal(1, cartItem.Id);
    }
}
