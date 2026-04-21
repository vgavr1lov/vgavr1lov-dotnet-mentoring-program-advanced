using ECommerce.Catalog.Application.Services;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using FluentAssertions;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class CategoryServiceIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    public CategoryServiceIntegrationTests(TestSqlServerFixture fixture)
    => Fixture = fixture;

    public TestSqlServerFixture Fixture { get; }

    private const string UpdatedCategoryName = "Updated Category Name";

    [Fact]
    public async Task GetCategories_WhenCategoriesAdded_ReturnAllCategories()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var catalogService = new CategoryService(context);
        var categories = GetSampleCategories();

        foreach (var category in categories)
        {
            await catalogService.AddCategory(category, CancellationToken.None);
        }

        // Act
        var result = await catalogService.GetAllCategories(CancellationToken.None);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, x => x.Id == categories[0].Id);
        Assert.Contains(result, x => x.Id == categories[1].Id);
    }

    [Fact]
    public async Task AddCategory_ShouldPersistCategory()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var catalogService = new CategoryService(context);
        var category = GetSampleCategory();

        // Act
        await catalogService.AddCategory(category, CancellationToken.None);
        var result = await catalogService.GetCategory(category.Id, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async Task UpdateCategory_ReturnUpdatedCategory()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var catalogService = new CategoryService(context);
        var category = GetSampleCategory();
        await catalogService.AddCategory(category, CancellationToken.None);

        // Act
        var updatedCategory = new Category(category.Id, UpdatedCategoryName, category.Image, category.ParentCategoryId);
        await catalogService.UpdateCategory(updatedCategory, CancellationToken.None);
        var result = await catalogService.GetCategory(category.Id, CancellationToken.None);

        // Assert
        Assert.Equal(UpdatedCategoryName, result?.Name);
    }

    [Fact]
    public async Task DeleteCategory_ReturnNoCategory()
    {
        // Arrange
        using var context = Fixture.CreateContext();
        var catalogService = new CategoryService(context);
        var category = GetSampleCategory();
        await catalogService.AddCategory(category, CancellationToken.None);

        // Act
        await catalogService.DeleteCategory(category.Id, CancellationToken.None);
        var result = await catalogService.GetCategory(category.Id, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    private Category GetSampleCategory()
    {
        return new Category(
            id: Random.Shared.NextInt64(1, long.MaxValue),
            name: "category",
            image: new Image("https://example.com/product.jpg", "product"),
            parentCategoryId: null);
    }



    private List<Category> GetSampleCategories()
    {
        var parentCategoryId = Random.Shared.NextInt64(1, long.MaxValue);

        return new List<Category> {
            new Category(
                id: parentCategoryId,
                name: "category",
                image: new Image("https://example.com/product.jpg", "product"),
                parentCategoryId: null),
            new Category(
                id: Random.Shared.NextInt64(1, long.MaxValue),
                name: "category",
                image: new Image("https://example.com/product.jpg", "product"),
                parentCategoryId: parentCategoryId) };
    }
}