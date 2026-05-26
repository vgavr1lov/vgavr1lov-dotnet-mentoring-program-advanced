using ECommerce.Catalog.Application.Products.Commands.DeleteProduct;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class DeleteProductCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly ApplicationDbContext _context;
    private readonly DeleteProductCommandHandler _sut;

    public DeleteProductCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();

        _sut = new DeleteProductCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WhenProductIsAddedThenDeleted_ShoulReturnNoProduct()
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

        // Act
        var deleteCommand = new DeleteProductCommand(product.Id);
        await _sut.Handle(deleteCommand, CancellationToken.None);
        var result = await _context.Product.FirstOrDefaultAsync(c => c.Id == product.Id);

        // Assert
        Assert.Null(result);
    }
}
