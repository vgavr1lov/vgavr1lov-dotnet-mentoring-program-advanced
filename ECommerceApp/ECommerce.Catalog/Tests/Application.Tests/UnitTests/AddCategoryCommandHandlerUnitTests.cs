using ECommerce.Catalog.Application.Categories.Commands.AddCategory;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Application.Tests.UnitTests;

public class AddCategoryCommandHandlerUnitTests
{
    private readonly AddCategoryCommandHandler _sut;

    public AddCategoryCommandHandlerUnitTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        _sut = new AddCategoryCommandHandler(context);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsAdded_ShouldReturnCategoryId()
    {
        // Arrange
        var category = new SampleCategoryBuilder()
            .WithRandomId()
            .Build();

        var request = new CreateCategoryRequest
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.Image?.Url,
            ImageAltText = category.Image?.AltText,
            ParentCategoryId = category.ParentCategoryId
        };

        var command = new AddCategoryCommand(request);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(category.Id, result);
    }
}