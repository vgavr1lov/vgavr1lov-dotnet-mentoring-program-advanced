using ECommerce.Cart.Domain.Entities;

namespace ECommerce.Cart.Application.Common.Interfaces;

public interface ICartRepository
{
    Task<ShoppingCart?> GetCartByIdAsync(long id, CancellationToken cancellationToken);
    Task SaveCartAsync(ShoppingCart cart, CancellationToken cancellationToken);
    Task SaveCartsAsync(List<ShoppingCart> carts, CancellationToken cancellationToken);
    Task<List<ShoppingCart>> GetCartsByItemId(long id, CancellationToken cancellationToken);
}
