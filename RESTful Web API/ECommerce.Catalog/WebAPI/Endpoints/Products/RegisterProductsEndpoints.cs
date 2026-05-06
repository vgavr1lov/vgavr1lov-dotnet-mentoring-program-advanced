using Asp.Versioning;

namespace ECommerce.Catalog.WebAPI.Endpoints.Products;

public static partial class EndpointsRegistration
{
    public static void RegisterProductsEndpoints(this WebApplication app)
    {
        var versionSet = app
            .NewApiVersionSet("CatalogAPIs")
            .ReportApiVersions()
            .Build();
        var version = new ApiVersion(1, 0);

        var group = app
            .MapGroup("/v{version:apiVersion}/products")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(version);

        Products.Map(group);
    }
}
