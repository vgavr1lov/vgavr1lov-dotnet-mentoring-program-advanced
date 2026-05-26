using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Domain.Entities;
using ECommerce.Cart.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Cart.Application.Carts.Commands.UpdateItem;

public record UpdateItemCommand(
    long Id,
    string Name,
    string? ImageUrl,
    string? ImageAltText,
    decimal Amount,
    string Currency
) : IRequest;

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly ICartRepository _cartRepository;

    public UpdateItemCommandHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var carts = await _cartRepository.GetCartsByItemId(command.Id, cancellationToken);

        foreach (var cart in carts)
        {
            var itemToUpdate = cart.Items
                .Where(item => item.Id == command.Id)
                .First();

            var item = new CartItem(
                id: command.Id,
                name: command.Name,
                image: string.IsNullOrWhiteSpace(command.ImageUrl)
                    ? null
                    : new Image(command.ImageUrl, command.ImageAltText),
                price: new Money(command.Amount, command.Currency),
                quantity: itemToUpdate.Quantity);

            cart.ReplaceItem(item);
        }

        await _cartRepository.SaveCartsAsync(carts, cancellationToken);
    }
}