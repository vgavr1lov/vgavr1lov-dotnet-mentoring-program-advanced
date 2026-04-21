using ECommerce.Cart.Application.Common.Interfaces;
using ECommerce.Cart.Domain.Entities;

namespace ECommerce.Cart.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public List<CartItem> GetItems(long cartId)
    {
        var cart = _cartRepository.GetCartById(cartId);
        var items = cart.Items;

        return items;
    }

    public void AddItem(long cartId, CartItem item)
    {
        var cart = _cartRepository.GetCartById(cartId);

        if (cart == null)
            cart = new ShoppingCart(cartId);

        cart.AddItem(item);

        _cartRepository.SaveCart(cart);
    }

    public void RemoveItem(long cartId, CartItem item)
    {
        var cart = _cartRepository.GetCartById(cartId);

        if (cart == null)
            return;

        cart.RemoveItem(item);
        _cartRepository.SaveCart(cart);
    }
}
