using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Domain.Tests.UnitTests;

public class CategoryUnitTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsBlank()
    {
        Assert.Throws<ArgumentException>(() => new Category(
            id: Random.Shared.NextInt64(1, long.MaxValue),
            name: "",
            image: new Image("https://example.com/category.jpg", "category"),
            parentCategoryId: null));
    }

    [Fact]
    public void Constructor_ShouldCreate_WhenIdAndNameAreProvided()
    {
        var categoryId = Random.Shared.NextInt64(1, long.MaxValue);

        var category = new Category(
            id: categoryId,
            name: "category",
            image: new Image("https://example.com/category.jpg", "category"),
            parentCategoryId: null);

        Assert.Equal(categoryId, category.Id);
    }
}
