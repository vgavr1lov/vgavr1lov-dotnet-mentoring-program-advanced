using ECommerce.Cart.Domain.ValueObjects;

namespace ECommerce.Cart.Domain.Entities;

public class CartItem
{
    public long Id { get; }
    public string Name { get; }
    public Image? Image { get; }
    public Money Price { get; }
    public int Quantity { get; private set; }

    public CartItem(long id, string name, Image? image, Money price, int quantity)
    {
        if (id == 0)
            throw new ArgumentException("Cart Item Id is required!");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Cart Item Name is required!");

        if (price is null)
            throw new ArgumentException("Cart Item Price is required!");

        Id = id;
        Name = name;
        Image = image;
        Price = price;
        Quantity = quantity;
    }

    public void IncreaseQuantity()
    {
        Quantity++;
    }

    public void IncreaseQuantity(int addend)
    {
        Quantity += addend;
    }

    public void DecreaseQuantity()
    {
        Quantity--;
    }

    public void DecreaseQuantity(int subtrahend)
    {
        Quantity -= subtrahend;
    }
}
