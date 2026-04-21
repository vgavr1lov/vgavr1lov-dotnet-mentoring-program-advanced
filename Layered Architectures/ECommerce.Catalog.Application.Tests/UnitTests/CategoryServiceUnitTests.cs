using ECommerce.Catalog.Application.Services;
using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;
using ECommerce.Catalog.Infrastructure.Data.Context;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.UnitTests;

public class CategoryServiceUnitTests
{
    [Fact]
    public async Task AddCategory_ShouldPersistCategory()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new ApplicationDbContext(options);
        var catalogService = new CategoryService(context);
        var sampleCategory = GetSampleCategory();

        // Act
        await catalogService.AddCategory(sampleCategory, CancellationToken.None);
        var actualResult = context.Category.FirstOrDefault(c => c.Id == sampleCategory.Id);

        // Assert 
        actualResult.Should().BeEquivalentTo(sampleCategory);
    }

    private Category GetSampleCategory()
    {
        return new Category(
            id: Random.Shared.NextInt64(1, long.MaxValue),
            name: "category",
            image: new Image("https://example.com/product.jpg", "product"),
            parentCategoryId: null);
    }
}
