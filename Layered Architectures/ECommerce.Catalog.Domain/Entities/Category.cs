using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Domain.Entities;

public class Category
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public Image? Image { get; private set; }
    public long? ParentCategoryId { get; private set; }

    public Category(long id, string name, Image? image, long? parentCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category Name is required!");

        if (name.Length > 50)
            throw new ArgumentException("Category Name cannot exceed 50 characters long!");

        Id = id;
        Name = name;
        Image = image;
        ParentCategoryId = parentCategoryId;
    }

    private Category() { }
}
