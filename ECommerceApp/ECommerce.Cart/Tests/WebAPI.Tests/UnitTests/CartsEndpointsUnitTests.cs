using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Carts.Queries.GetCartInfo;
using ECommerce.Cart.WebAPI.Endpoints.Carts;
using ECommerce.Cart.WebAPI.Tests.TestDataBuilders;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ECommerce.Cart.WebAPI.Tests.UnitTests;

public class CartsEndpointsUnitTests
{
    private const long SampleCartId = 1;

    [Fact]
    public async Task GetCartInfo_WhenCartExists_ReturnsCartItems()
    {
        // Arrange
        var senderMock = new Mock<ISender>();

        var cartItem = new SampleCartItemBuilder()
            .WithRandomId()
            .Build();

        var items = new List<ItemModel> { cartItem };

        senderMock
            .Setup(s => s.Send(It.IsAny<GetCartInfoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CartModel { CartId = SampleCartId, Items = items });

        // Act
        var result = await CartsV1.GetCartInfo(
            SampleCartId,
            senderMock.Object,
            CancellationToken.None);

        // Assert
        var okResult = result as Ok<CartModel>;
        okResult.Should().NotBeNull();
        okResult.Value?.Items.Should().BeEquivalentTo(items);
    }
}