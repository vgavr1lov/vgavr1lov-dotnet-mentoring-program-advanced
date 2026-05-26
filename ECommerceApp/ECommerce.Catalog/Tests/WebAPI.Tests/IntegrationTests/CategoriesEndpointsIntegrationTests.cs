using ECommerce.Catalog.Application.Categories.Contracts;
using ECommerce.Catalog.WebAPI.Contracts;
using ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace ECommerce.Catalog.WebAPI.Tests.IntegrationTests;

public class CategoriesEndpointsIntegrationTests :
    IClassFixture<CustomWebAPIApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private const string ApiVersion = "/v1";

    public CategoriesEndpointsIntegrationTests(
        CustomWebAPIApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCategory_WhenCategoryExists_ReturnsCategory()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"{ApiVersion}/categories/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var category = await response.Content.ReadFromJsonAsync<CategoryModel>();
        category.Should().NotBeNull();
        category.Id.Should().Be(1);
    }

    [Fact]
    public async Task AddCategory_WhenCategoryIsAdded_ShouldReturnCategoryId()
    {
        // Arrange
        var categoryModel = new SampleCategoryModelBuilder()
            .WithRandomId()
            .Build();

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiVersion}/categories/", categoryModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var resource = await response.Content.ReadFromJsonAsync<Resource<long>>();
        resource?.Data.Should().Be(categoryModel.Id);
    }
}