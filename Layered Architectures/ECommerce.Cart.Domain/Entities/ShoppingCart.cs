namespace ECommerce.Cart.Domain.Entities;

public class ShoppingCart
{
    public long Id { get; private set; }
    public List<CartItem> Items { get; private set; }

    public ShoppingCart(long id)
    {
        Id = id;
        Items = new List<CartItem>();
    }
    public void RemoveItem(CartItem item)
    {
        var existingItem = Items.FirstOrDefault(x => x.Id == item.Id);

        if (existingItem != null)
        {
            existingItem.DecreaseQuantity();

            if (existingItem.Quantity <= 0)
                Items.Remove(existingItem);
        }
    }

    public void AddItem(CartItem item)
    {
        var existingItem = Items.FirstOrDefault(x => x == item);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantity();
        }
        else
        {
            Items.Add(item);
        }
    }
}