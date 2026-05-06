using ECommerce.Catalog.WebAPI.Interfaces;

public class LinkService : ILinkService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public LinkService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string? GenerateLink(string routeName, object? routeValues)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException();

        return _linkGenerator.GetUriByName(httpContext, routeName, routeValues);
    }
}
