using ECommerce.Catalog.Application.Categories.Commands.UpdateCategory;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class UpdateCategoryCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly ApplicationDbContext _context;
    private readonly UpdateCategoryCommandHandler _sut;

    public UpdateCategoryCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CreateContext();

        _sut = new UpdateCategoryCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsAddedThenUpdatedCategoryName_ShouldPersistAndReturnUpdatedCategory()
    {
        // Arrange
        var category = new SampleCategoryBuilder()
            .WithRandomId()
            .Build();

        await _context.Category.AddAsync(category);
        await _context.SaveChangesAsync(CancellationToken.None);

        var updatedCategoryRequest = new UpdateCategoryRequest
        {
            Id = category.Id,
            Name = "Updated Name",
            ImageUrl = category.Image?.Url,
            ImageAltText = category.Image?.AltText,
            ParentCategoryId = category.ParentCategoryId
        };

        var updateCategoryCommand = new UpdateCategoryCommand(updatedCategoryRequest);

        // Act
        await _sut.Handle(updateCategoryCommand, CancellationToken.None);
        var result = await _context.Category.FirstOrDefaultAsync(c => c.Id == category.Id);

        // Assert
        Assert.Equal(updatedCategoryRequest.Name, result?.Name);
    }
}
