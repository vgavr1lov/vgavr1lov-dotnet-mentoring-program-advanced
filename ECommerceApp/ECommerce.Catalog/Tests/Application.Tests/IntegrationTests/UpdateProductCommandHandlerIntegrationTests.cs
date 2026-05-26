using ECommerce.Catalog.Application.Products.Commands.UpdateProduct;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class UpdateProductCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;

    public UpdateProductCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Handle_WhenProductIsAddedThenUpdatedProductName_ShouldPersistAndReturnUpdatedProduct()
    {
        // Arrange
        var category = new SampleCategoryBuilder()
            .WithRandomId()
            .Build();

        var product = new SampleProductBuilder()
            .WithRandomId()
            .WithCategoryId(category.Id)
            .Build();

        await using (var seedContext = _fixture.CreateContext())
        {
            await seedContext.Category.AddAsync(category);
            await seedContext.SaveChangesAsync(CancellationToken.None);
            await seedContext.Product.AddAsync(product);
            await seedContext.SaveChangesAsync(CancellationToken.None);
        }

        var updatedProductRequest = new UpdateProductRequest
        {
            Id = product.Id,
            Name = "Updated Name",
            Description = product.Description,
            ImageUrl = product.Image?.Url,
            ImageAltText = product.Image?.AltText,
            CategoryId = category.Id,
            Amount = product.Price.Amount,
            Currency = product.Price.Currency,
            Quantity = product.Amount
        };

        var updateProductCommand = new UpdateProductCommand(updatedProductRequest);

        // Act
        await using (var actContext = _fixture.CreateContext())
        {
            var sut = new UpdateProductCommandHandler(actContext);
            await sut.Handle(updateProductCommand, CancellationToken.None);
        }

        // Assert
        await using (var assertContext = _fixture.CreateContext())
        {
            var result = await assertContext.Product.FirstOrDefaultAsync(c => c.Id == product.Id);
            Assert.Equal(updatedProductRequest.Name, result?.Name);
        }
    }
}