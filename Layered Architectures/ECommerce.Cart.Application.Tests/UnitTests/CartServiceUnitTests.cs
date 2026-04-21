using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Application.Services;
using ECommerce.Cart.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ECommerce.Cart.Application.Tests.UnitTests;

public class CartServiceUnitTests
{
    private const long SampleCartId = 1;

    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly CartService _sut;

    public CartServiceUnitTests()
    {
        _cartRepositoryMock = new Mock<ICartRepository>();
        _sut = new CartService(_cartRepositoryMock.Object);
    }

    [Fact]
    public void GetItems_WhenItemsExist_ReturnItems()
    {
        // Arrange
        var expectedResult = GetSampleCartItems();
        var cart = new ShoppingCart(SampleCartId);

        foreach (var item in GetSampleCartItems())
        {
            cart.AddItem(item);
        }

        _cartRepositoryMock
            .Setup(x => x.GetCartById(SampleCartId))
            .Returns(cart);

        // Act
        var actualResult = _sut.GetItems(SampleCartId);

        // Assert 
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    private List<CartItem> GetSampleCartItems()
    {
        return new List<CartItem> {
            new CartItem(
                id: 1,
                name:  "product1",
                image:  new Domain.ValueObjects.Image("https://example.com/product1.jpg", "product1"),
                price:  new Domain.ValueObjects.Money(10m, "PLN"),
                quantity:  1 ),
            new CartItem(
                id:  2,
                name:  "product2",
                image:  new Domain.ValueObjects.Image("https://example.com/product2.jpg", "product2"),
                price:  new Domain.ValueObjects.Money(20.777m, "PLN"),
                quantity:  2 ),
            new CartItem(
                id:  3,
                name:  "product3",
                image:  new Domain.ValueObjects.Image("https://example.com/product3.jpg", "product3"),
                price: new Domain.ValueObjects.Money(30m, "PLN"),
                quantity:  3 )};
    }
}
