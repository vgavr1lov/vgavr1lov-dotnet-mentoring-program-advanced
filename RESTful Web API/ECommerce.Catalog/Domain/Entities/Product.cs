using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Domain.Entities;

public class Product
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Image? Image { get; private set; }
    public long CategoryId { get; private set; }
    public Money Price { get; private set; }
    public int Amount { get; private set; }

    public Product(long id, string name, string? description, Image? image, long categoryId, Money price, int amount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product Name is required!");

        if (name.Length > 50)
            throw new ArgumentException("Product Name cannot exceed 50 characters long!");

        if (categoryId == 0)
            throw new ArgumentException("Product Category is required!");

        if (price is null)
            throw new ArgumentException("Product Price is required!");

        if (amount < 0)
            throw new ArgumentException("Product Amount cannot be negative!");

        if (amount == 0)
            throw new ArgumentException("Product Amount is required!");



        Id = id;
        Name = name;
        Description = description;
        Image = image;
        CategoryId = categoryId;
        Price = price;
        Amount = amount;
    }

    private Product() { }
}
