using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Services;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using ECommerce.Catalog.Infrastructure.Data.Context;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.UnitTests;

public class ProductServiceUnitTests
{
    [Fact]
    public async Task AddProduct_ShouldPersistProduct()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var productService = new ProductService(context);
        var categoryId = await CreateSampleCategoryAsync(context);
        var sampleProduct = GetSampleProduct(categoryId);

        // Act
        await productService.AddProduct(sampleProduct, CancellationToken.None);
        var actualResult = context.Product.FirstOrDefault(c => c.Id == sampleProduct.Id);

        // Assert 
        actualResult.Should().BeEquivalentTo(sampleProduct);
    }

    private Product GetSampleProduct(long categoryId)
    {
        return new Product(
                id: Random.Shared.NextInt64(1, long.MaxValue),
                name: "Product",
                description: "Product Description",
                image: new Image("https://example.com/product.jpg", "product"),
                categoryId: categoryId,
                price: new Money(10.20m, "PLN"),
                amount: 1);
    }

    private async Task<long> CreateSampleCategoryAsync(IApplicationDbContext context)
    {
        var category = new Category(
            id: Random.Shared.NextInt64(1, long.MaxValue),
            name: "category",
            image: new Image("https://example.com/product.jpg", "product"),
            parentCategoryId: null);

        var catalogService = new CategoryService(context);

        await catalogService.AddCategory(category, CancellationToken.None);

        return category.Id;
    }
}
