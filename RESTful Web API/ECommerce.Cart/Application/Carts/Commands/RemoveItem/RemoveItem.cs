using ECommerce.Cart.Application.Carts.Contracts;
using ECommerce.Cart.Application.Common.Exceptions;
using ECommerce.Cart.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Cart.Application.Carts.Commands.RemoveItem;

public record RemoveItemCommand(RemoveItemRequest Request) : IRequest;

public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand>
{
    private readonly ICartRepository _cartRepository;

    public RemoveItemCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(RemoveItemCommand command, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetCartByIdAsync(command.Request.CartId, cancellationToken);

        if (cart == null)
            throw new NotFoundException("Cart not found.");

        if (!cart.Items.Any(i => i.Id == command.Request.ItemId))
            throw new NotFoundException("Item not found in cart.");

        cart.RemoveItem(command.Request.ItemId);
        await _cartRepository.SaveCartAsync(cart, cancellationToken);
    }
}