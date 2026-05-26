using ECommerce.Cart.Domain.ValueObjects;

namespace ECommerce.Cart.Domain.Tests.UnitTests;

public class ImageUnitTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenUrlIsBlank()
    {
        Assert.Throws<ArgumentException>(() => new Image(
            url: "",
            altText: ""));
    }

    [Fact]
    public void Constructor_ShouldCreate_WhenValidUrlIsProvided()
    {
        var url = "https://example.com/product.jpg";

        var image = new Image(
            url: url,
            altText: "product");

        Assert.Equal(url, image.Url);
    }
}
