using ECommerce.Catalog.Domain.Entities;
using ECommerce.Catalog.Domain.ValueObjects;

namespace ECommerce.Catalog.Application.Tests.TestDataBuilders;

public class SampleCategoryBuilder
{
    private long _id = 1;
    private string _name = "category";
    private Image? _image = new Image("https://example.com/product.jpg", "product");
    private long? _parentCategoryId = null;


    public SampleCategoryBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public Category Build() => new Category(_id, _name, _image, _parentCategoryId);
}
