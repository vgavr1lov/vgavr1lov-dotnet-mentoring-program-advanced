using ECommerce.Catalog.Application.Products.Commands.UpdateProduct;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class UpdateProductCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly ApplicationDbContext _context;
    private readonly UpdateProductCommandHandler _sut;

    public UpdateProductCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();

        _sut = new UpdateProductCommandHandler(_context);
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

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync(CancellationToken.None);

        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync(CancellationToken.None);

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
        await _sut.Handle(updateProductCommand, CancellationToken.None);
        var result = await _context.Product.FirstOrDefaultAsync(c => c.Id == product.Id);

        // Assert
        Assert.Equal(updatedProductRequest.Name, result?.Name);
    }
}