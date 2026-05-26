namespace ECommerce.Catalog.Application.Products.Contracts;

public class GetProductsRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long? CategoryId { get; set; }
}
