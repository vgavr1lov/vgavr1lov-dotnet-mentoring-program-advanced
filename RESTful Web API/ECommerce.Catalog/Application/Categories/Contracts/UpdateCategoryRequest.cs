namespace ECommerce.Catalog.Application.Categories.Contracts;

public class UpdateCategoryRequest
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageAltText { get; set; }
    public long? ParentCategoryId { get; set; }
}
