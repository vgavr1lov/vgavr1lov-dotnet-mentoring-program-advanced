using ECommerce.Catalog.Application.Products.Contracts;

namespace ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;

public class SampleProductModelBuilder
{
    private long _id = 1;
    private readonly string _name = "Product";
    private readonly string _description = "Product Description";
    private readonly string? _imageUrl = "https://example.com/product.jpg";
    private readonly string? _imageAltText = "Product";
    private long _categoryId = 1;
    private readonly decimal _amount = 10.20m;
    private readonly string _currency = "PLN";
    private readonly int _quantity = 1;

    public SampleProductModelBuilder WithRandomId()
    {
        _id = Random.Shared.NextInt64(1, long.MaxValue);
        return this;
    }

    public SampleProductModelBuilder WithCategoryId(long categoryId)
    {
        _categoryId = categoryId;
        return this;
    }

    public ProductModel Build() => new ProductModel
    {
        Id = _id,
        Name = _name,
        Description = _description,
        ImageUrl = _imageUrl,
        ImageAltText = _imageAltText,
        CategoryId = _categoryId,
        Amount = _amount,
        Currency = _currency,
        Quantity = _quantity,
    };
}
