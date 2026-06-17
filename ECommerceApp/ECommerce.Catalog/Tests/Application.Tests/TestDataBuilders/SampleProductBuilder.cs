using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Application.Tests.TestDataBuilders;

public class SampleProductBuilder
{
    private long _id = 1;
    private readonly string _name = "Product";
    private readonly string _description = "Product Description";
    private readonly Image? _image = new Image("https://example.com/product.jpg", "product");
    private long _categoryId = 1;
    private readonly Money _price = new Money(10.20m, "PLN");
    private readonly int _quantity = 1;

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
