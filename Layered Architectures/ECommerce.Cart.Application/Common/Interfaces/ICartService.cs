using ECommerce.Cart.Domain.Entities;

namespace ECommerce.Cart.Application.Common.Interfaces;

public interface ICartService
{
    void AddItem(long cartId, CartItem item);
    List<CartItem> GetItems(long cartId);
    void RemoveItem(long cartId, CartItem item);
}
