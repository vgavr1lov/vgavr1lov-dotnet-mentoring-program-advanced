using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Products.Commands.AddProduct;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class AddProductCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly AddProductCommandHandler _sut;
    private readonly IApplicationDbContext _context;

    public AddProductCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();
        _sut = new AddProductCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WhenProductIsAdded_ShouldReturnProductId()
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
