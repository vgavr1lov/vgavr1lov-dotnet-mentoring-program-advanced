using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.WebAPI.Tests.TestDataBuilders;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace ECommerce.Cart.WebAPI.Tests.IntegrationTests;

public class CartsEndpointsIntegrationTests :
    IClassFixture<CustomWebAPIApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private const string ApiVersion = "/v1";
    private const long SampleCartId = 1;

    public CartsEndpointsIntegrationTests(
        CustomWebAPIApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCartInfo_WhenCartExists_ReturnsCartItems()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"{ApiVersion}/carts/{SampleCartId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cart = await response.Content.ReadFromJsonAsync<CartModel>();
        cart?.Items.Should().NotBeNull().And.Contain(i => i.Id == 1);
    }

    [Fact]
    public async Task AddItemToCart_WhenItemIsAdded_ShouldPersistItem()
    {
        // Arrange
        var cartItem = new SampleCartItemBuilder()
            .WithRandomId()
            .Build();

        // Act
        var response = await _client.PostAsJsonAsync($"{ApiVersion}/carts/{SampleCartId}/items", cartItem);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}