namespace ECommerce.Catalog.WebAPI.Interfaces;

public interface ILinkService
{
    string? GenerateLink(string routeName, object? routeValues);
}
