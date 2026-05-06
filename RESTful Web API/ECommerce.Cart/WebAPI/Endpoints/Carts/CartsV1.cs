using ECommerce.Cart.Application.Carts.Commands.AddItem;
using ECommerce.Cart.Application.Carts.Commands.RemoveItem;
using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Carts.Queries.GetCartInfo;
using ECommerce.Cart.WebAPI.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Cart.WebAPI.Endpoints.Carts;

public class CartsV1 : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/{cartId:long}", GetCartInfo); 
        groupBuilder.MapPost("/{cartId:long}/items", AddItemToCart);
        groupBuilder.MapDelete("/{cartId:long}/items/{itemId:long}", DeleteItemFromCart);
    }

    /// <summary>
    /// Get Cart Info.
    /// </summary>
    /// <param name="cartId">The Cart Id.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /carts/1
    ///
    /// </remarks>
    /// <response code="200">Returns Ok</response>
    /// <response code="404">Returns Not Found</response>
    [EndpointSummary("Get a Cart")]
    [EndpointDescription("Gets a Cart and returns a Cart with items.")]
    public static async Task<IResult> GetCartInfo(
        long cartId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetCartInfoQuery(cartId), cancellationToken);

        if (response == null) 
            return Results.NotFound();

        return Results.Ok(response);
    }

    /// <summary>
    /// Adds item to a Cart.
    /// </summary>
    /// <param name="cartId">The Cart Id.</param>
    /// <param name="item">The Cart item to add.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /carts/1/items
    ///     {
    ///       "id": 1,
    ///       "name": "product",
    ///       "imageUrl": "https://example.com/product.jpg",
    ///       "imageAltText": "product",
    ///       "amount": 10,
    ///       "currency": "PLN",
    ///       "quantity": 1
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns Ok</response>
    public static async Task<IResult> AddItemToCart(
        long cartId,
        [FromBody] ItemModel item,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var request = new AddItemRequest
        {
            CartId = cartId,
            ItemId = item.Id,
            Name = item.Name,
            ImageUrl = item.ImageUrl,
            ImageAltText = item.ImageAltText,
            Amount = item.Amount,
            Currency = item.Currency,
            Quantity = item.Quantity
        };

        var command = new AddItemCommand(request);
        await sender.Send(command, cancellationToken);

        return Results.Ok();
    }

    /// <summary>
    /// Delete Cart Item.
    /// </summary>
    /// <param name="cartId">Cart Id.</param>
    /// <param name="itemId">Item Id.</param>
    /// <returns>Ok</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /carts/1/items/1
    ///
    /// </remarks>
    /// <response code="200">Returns Ok</response>
    /// <response code="404">Returns Not Found</response>
    [EndpointSummary("Delete a Cart item")]
    [EndpointDescription("Delete a Cart item and returns Ok.")]
    public static async Task<IResult> DeleteItemFromCart(
    long cartId,
    long itemId,
    [FromServices] ISender sender,
    CancellationToken cancellationToken)
    {
        var request = new RemoveItemRequest { CartId = cartId, ItemId = itemId };
        var command = new RemoveItemCommand(request);
        await sender.Send(command, cancellationToken);

        return Results.Ok();
    }
}
