using ECommerce.Catalog.Application.Categories.Contracts;

namespace ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;

public class SampleCategoryModelBuilder
{
    private long _id = 1;
    private string _name = "category";
    private string? _imageUrl = "https://example.com/product.jpg";
    private string? _imageAltText = "category";
    private long? _parentCategoryId = null;


    public SampleCategoryModelBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public CategoryModel Build() => new CategoryModel { 
        Id = _id, 
        Name = _name, 
        ImageUrl = _imageUrl, 
        ImageAltText = _imageAltText, 
        ParentCategoryId = _parentCategoryId };
}
