using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Application.Tests.TestDataBuilders;

public class SampleProductBuilder
{
    private long _id = 1;
    private string _name = "Product";
    private string _description = "Product Description";
    private Image? _image = new Image("https://example.com/product.jpg", "product");
    private long _categoryId = 1;
    private Money _price = new Money(10.20m, "PLN");
    private int _quantity = 1;

    public SampleProductBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public SampleProductBuilder WithCategoryId(long categoryId)
    {
        _categoryId = categoryId;
        return this;
    }

    public Product Build() => new Product(_id, _name, _description, _image, _categoryId, _price, _quantity);
}