namespace ECommerce.Catalog.WebAPI.Contracts;

public class Resource<T>
{
    public required T Data { get; set; }
    public List<LinkResource> Links { get; set; } = [];
}
