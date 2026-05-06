using ECommerce.Cart.Domain.Entities;

namespace ECommerce.Cart.Application.Common.Interfaces;

public interface ICartRepository
{
    Task<ShoppingCart?> GetCartByIdAsync(long id, CancellationToken cancellationToken);
    Task SaveCartAsync(ShoppingCart cart, CancellationToken cancellationToken);
}
