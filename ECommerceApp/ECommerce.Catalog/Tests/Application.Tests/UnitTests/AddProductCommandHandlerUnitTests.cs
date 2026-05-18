using ECommerce.Catalog.Application.Products.Commands.AddProduct;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.UnitTests;

public class AddProductCommandHandlerUnitTests
{
    private readonly AddProductCommandHandler _sut;

    public AddProductCommandHandlerUnitTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        _sut = new AddProductCommandHandler(context);
    }

    [Fact]
    public async Task Handle_WhenProductIsAdded_ShouldReturnProductId()
    {
        // Arrange
        var product = new SampleProductBuilder()
            .WithRandomId()
            .Build();

        var request = new CreateProductRequest
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.Image?.Url,
            ImageAltText = product.Image?.AltText,
            CategoryId = product.CategoryId,
            Amount = product.Price.Amount,
            Currency = product.Price.Currency,
            Quantity = product.Amount
        };

        var command = new AddProductCommand(request);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(product.Id, result);
    }
}
