using ECommerce.Catalog.Application.Services;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using FluentAssertions;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class ProductServiceIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    public ProductServiceIntegrationTests(TestSqlServerFixture fixture)
    => Fixture = fixture;

    public TestSqlServerFixture Fixture { get; }

    private const string UpdatedProductName = "Updated Product Name";

    [Fact]
    public async Task GetProducts_WhenProductsAdded_ReturnAllProducts()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var productService = new ProductService(context);
        var products = await GetSampleProductsAsync();

        foreach (var product in products)
        {
            await productService.AddProduct(product, CancellationToken.None);
        }

        // Act
        var result = await productService.GetAllProducts(CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, x => x.Id == products[0].Id);
        Assert.Contains(result, x => x.Id == products[1].Id);
    }

    [Fact]
    public async Task AddProduct_ShouldPersistProduct()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var productService = new ProductService(context);
        var product = await GetSampleProductAsync();

        // Act
        await productService.AddProduct(product, CancellationToken.None);
        var result = await productService.GetProduct(product.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task UpdateProduct_ReturnUpdatedProduct()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var productService = new ProductService(context);
        var product = await GetSampleProductAsync();
        await productService.AddProduct(product, CancellationToken.None);

        // Act
        var updatedProduct = new Product(
            product.Id,
            UpdatedProductName,
            product?.Description,
            product?.Image,
            product!.CategoryId,
            product.Price,
            product.Amount);
        await productService.UpdateProduct(updatedProduct, CancellationToken.None);
        var result = await productService.GetProduct(product.Id, CancellationToken.None);

        // Assert
        Assert.Equal(UpdatedProductName, result?.Name);
    }

    [Fact]
    public async Task DeleteProduct_ReturnNoProduct()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var productService = new ProductService(context);
        var product = await GetSampleProductAsync();
        await productService.AddProduct(product, CancellationToken.None);

        // Act
        await productService.DeleteProduct(product.Id, CancellationToken.None);
        var result = await productService.GetProduct(product.Id, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    private async Task<Product> GetSampleProductAsync()
    {
        var categoryId = await CreateSampleCategoryAsync();

        return new Product(
                id: Random.Shared.NextInt64(1, long.MaxValue),
                name: "Product",
                description: "Product Description",
                image: new Image("https://example.com/product.jpg", "product"),
                categoryId: categoryId,
                price: new Money(10.20m, "PLN"),
                amount: 1);
    }

    private async Task<List<Product>> GetSampleProductsAsync()
    {
        var categoryId = await CreateSampleCategoryAsync();

        return new List<Product> {
            new Product(
                id: Random.Shared.NextInt64(1, long.MaxValue),
                name: "Product",
                description: "Product Description",
                image: new Image("https://example.com/product.jpg", "product"),
                categoryId: categoryId,
                price: new Money(10.20m, "PLN"),
                amount: 1),
            new Product(
                 id: Random.Shared.NextInt64(1, long.MaxValue),
                name: "Product",
                description: "Product Description",
                image: new Image("https://example.com/product.jpg", "product"),
                categoryId: categoryId,
                price: new Money(10.20m, "PLN"),
                amount: 1) };
    }

    private async Task<long> CreateSampleCategoryAsync()
    {
        var category = new Category(
            id: Random.Shared.NextInt64(1, long.MaxValue),
            name: "category",
            image: new Image("https://example.com/product.jpg", "product"),
            parentCategoryId: null);

        using var context = Fixture.CreateContext();
        var catalogService = new CategoryService(context);

        await catalogService.AddCategory(category, CancellationToken.None);

        return category.Id;
    }
}
