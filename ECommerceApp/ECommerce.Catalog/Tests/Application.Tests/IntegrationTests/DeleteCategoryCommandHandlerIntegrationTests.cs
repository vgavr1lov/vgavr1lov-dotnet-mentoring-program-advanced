using ECommerce.Catalog.Application.Categories.Commands.DeleteCategory;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class DeleteCategoryCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly ApplicationDbContext _context;
    private readonly DeleteCategoryCommandHandler _sut;

    public DeleteCategoryCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();

        _sut = new DeleteCategoryCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsAddedThenDeleted_ShoulReturnNoCategory()
    {
        // Arrange
        var category = new SampleCategoryBuilder()
            .WithRandomId()
            .Build();

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync(CancellationToken.None);

        // Act
        var deleteCommand = new DeleteCategoryCommand(category.Id);
        await _sut.Handle(deleteCommand, CancellationToken.None);
        var result = await _context.Category.FirstOrDefaultAsync(c => c.Id == category.Id);

        // Assert
        Assert.Null(result);
    }
}
