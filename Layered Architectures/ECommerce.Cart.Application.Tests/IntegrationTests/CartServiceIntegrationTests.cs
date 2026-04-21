using ECommerce.Cart.Application.Services;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Infrastructure.Repositories;

namespace ECommerce.Cart.Application.Tests.IntegrationTests;

public class CartServiceIntegrationTests: IDisposable
{
    private const long SampleCartId = 1;

    private readonly string _dbPath;
    private readonly CartRepository _cartTestRepository;
    private readonly CartService _sut;

    public CartServiceIntegrationTests()
    {
        _dbPath = Path.Combine(
            Path.GetTempPath(),
            $"cart_test_{Guid.NewGuid():N}.db");

        _cartTestRepository = new CartRepository(_dbPath);
        _sut = new CartService(_cartTestRepository);
    }

    [Fact]
    public void GetItems_WhenItemAdded_ReturnsItem()
    {
        // Arrange
        var item = GetSampleCartItem();

        _sut.AddItem(SampleCartId, item);

        // Act
        var result = _sut.GetItems(SampleCartId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Single(result);
    }

    [Fact]
    public void AddItem_ShouldPersistItem()
    {
        // Arrange
        var item = GetSampleCartItem();

        // Act
        _sut.AddItem(SampleCartId, item);
        var result = _sut.GetItems(SampleCartId);

        // Assert
        Assert.Contains(result, x => x.Id == item.Id);
    }

    [Fact]
    public void RemoveItem_ShouldDeleteItem()
    {
        // Arrange
        var item = GetSampleCartItem();
        _sut.AddItem(SampleCartId, item);

        // Act
        _sut.RemoveItem(SampleCartId, item);
        var result = _sut.GetItems(SampleCartId);

        // Assert
        Assert.DoesNotContain(result, x => x.Id == item.Id);
    }

    public void Dispose()
    {
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch
        {          
        }
    }

    private CartItem GetSampleCartItem()
    {
        return new CartItem(
            id: 1,
            name: "product1",
            image: new Domain.ValueObjects.Image("https://example.com/product1.jpg", "product1"),
            price: new Domain.ValueObjects.Money(10m, "PLN"),
            quantity: 1);
    }
}
