namespace ECommerce.Catalog.WebAPI.Contracts;

public class LinkResource
{
    public string? Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
}
