using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Cart.Application.Carts.Commands.AddItem;

public record AddItemCommand(AddItemRequest Request) : IRequest;

public class AddItemCommandHandler : IRequestHandler<AddItemCommand>
{
    private readonly ICartRepository _cartRepository;

    public AddItemCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(AddItemCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByIdAsync(command.Request.CartId, cancellationToken);

        if (cart == null)
            cart = new ShoppingCart(command.Request.CartId);

        var item = new CartItem(
            id: command.Request.ItemId,
            name: command.Request.Name,
            image: string.IsNullOrWhiteSpace(command.Request.ImageUrl)
                ? null
                : new Image(command.Request.ImageUrl, command.Request.ImageAltText),
            price: new Money(command.Request.Amount, command.Request.Currency),
            quantity: command.Request.Quantity
        );

        cart.AddItem(item);

        await _cartRepository.SaveCartAsync(cart, cancellationToken);
    }
}