using ECommerce.Catalog.Application.Categories.Contracts;

namespace ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;

public class SampleCategoryModelBuilder
{
    private long _id = 1;
    private readonly string _name = "category";
    private readonly string? _imageUrl = "https://example.com/product.jpg";
    private readonly string? _imageAltText = "category";

    public SampleCategoryModelBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public CategoryModel Build() => new CategoryModel
    {
        Id = _id,
        Name = _name,
        ImageUrl = _imageUrl,
        ImageAltText = _imageAltText,
        ParentCategoryId = null,
    };
}
