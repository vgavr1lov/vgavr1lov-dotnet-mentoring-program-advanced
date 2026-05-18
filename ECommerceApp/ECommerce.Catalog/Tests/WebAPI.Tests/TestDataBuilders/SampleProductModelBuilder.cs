using ECommerce.Catalog.Application.Products.Contracts;

namespace ECommerce.Catalog.WebAPI.Tests.TestDataBuilders;

public class SampleProductModelBuilder
{
    private long _id = 1;
    private string _name = "Product";
    private string _description = "Product Description";
    private string? _imageUrl = "https://example.com/product.jpg";
    private string? _imageAltText = "Product";
    private long _categoryId = 1;
    private decimal _amount = 10.20m;
    private string _currency = "PLN";
    private int _quantity = 1;

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
        Quantity = _quantity
    };
}
