using ECommerce.Catalog.Application.Categories.Commands.AddCategory;
using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Tests.TestDataBuilders;

namespace ECommerce.Catalog.Application.Tests.IntegrationTests;

public class AddCategoryCommandHandlerIntegrationTests : IClassFixture<TestSqlServerFixture>
{
    private readonly TestSqlServerFixture _fixture;
    private readonly AddCategoryCommandHandler _sut;

    public AddCategoryCommandHandlerIntegrationTests(TestSqlServerFixture fixture)
    {
        _fixture = fixture;
        _sut = new AddCategoryCommandHandler(_fixture.CreateContext());
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
