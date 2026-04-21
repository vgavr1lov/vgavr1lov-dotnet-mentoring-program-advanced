using ECommerce.Cart.Domain.Entities;

namespace ECommerce.Cart.Application.Common.Interfaces;

public interface ICartRepository
{
    ShoppingCart GetCartById(long id);
    void SaveCart(ShoppingCart cart);
}
