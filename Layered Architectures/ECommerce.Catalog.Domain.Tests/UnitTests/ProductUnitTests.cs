using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Domain.Tests.UnitTests;

public class ProductUnitTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsBlank()
    {
        Assert.Throws<ArgumentException>(() => new Product(
                id: Random.Shared.NextInt64(1, long.MaxValue),
                name: "",
                description: "Product Description",
                image: new Image("https://example.com/product.jpg", "product"),
                categoryId: 1,
                price: new Money(10.20m, "PLN"),
                amount: 1));
    }

    [Fact]
    public void Constructor_ShouldCreate_WhenAllRequiredDataIsProvided()
    {
        var productId = Random.Shared.NextInt64(1, long.MaxValue);

        var product = new Product(
                id: productId,
                name: "Product",
                description: "Product Description",
                image: new Image("https://example.com/product.jpg", "product"),
                categoryId: 1,
                price: new Money(10.20m, "PLN"),
                amount: 1);

        Assert.Equal(productId, product.Id);
    }
}
