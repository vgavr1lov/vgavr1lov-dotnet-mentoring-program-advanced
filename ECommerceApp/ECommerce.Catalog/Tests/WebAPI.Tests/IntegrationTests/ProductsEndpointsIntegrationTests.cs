using System.Net;
using System.Net.Http.Json;
using ECommerce.Catalog.Application.Products.Contracts;
using ECommerce.Catalog.WebAPI.Contracts;
using ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;

using FluentAssertions;

namespace ECommerce.Catalog.WebAPI.Tests.IntegrationTests;

public class ProductsEndpointsIntegrationTests :
    IClassFixture<CustomWebAPIApplicationFactory<Program>>
{
    private const string ApiVersion = "/v1";

    private readonly HttpClient _client;

    public ProductsEndpointsIntegrationTests(
        CustomWebAPIApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ReturnsProduct()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"{ApiVersion}/products/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var product = await response.Content.ReadFromJsonAsync<ProductModel>();
        product.Should().NotBeNull();
        product.Id.Should().Be(1);
    }

    [Fact]
    public async Task AddProduct_WhenProductIsAdded_ShouldReturnProductId()
    {
        // Arrange
        var productModel = new SampleProductModelBuilder()
            .WithRandomId()
            .Build();

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiVersion}/products/", productModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var resource = await response.Content.ReadFromJsonAsync<Resource<long>>();
        resource?.Data.Should().Be(productModel.Id);
    }
}
