using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.Application.Categories.Queries.GetCategory;
using ECommerce.Catalog.WebAPI.Endpoints.Categories;
using ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ECommerce.Catalog.WebAPI.Tests.UnitTests;

public class CategoriesEndpointsUnitTests
{
    [Fact]
    public async Task GetCategory_WhenCategoryExists_ReturnsCategory()
    {
        // Arrange
        var senderMock = new Mock<ISender>();

        var category = new SampleCategoryModelBuilder()
            .WithRandomId()
            .Build();

        senderMock
            .Setup(s => s.Send(It.IsAny<GetCategoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await Categories.GetCategory(
            category.Id,
            senderMock.Object,
            CancellationToken.None);

        // Assert
        var okResult = result as Ok<CategoryModel>;
        okResult.Should().NotBeNull();
        okResult.Value?.Id.Should().Be(category.Id);
    }
}