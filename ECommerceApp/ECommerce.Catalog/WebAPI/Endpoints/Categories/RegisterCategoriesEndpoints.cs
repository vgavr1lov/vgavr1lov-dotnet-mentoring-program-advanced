using Asp.Versioning;
using ECommerce.Catalog.WebAPI.Authorization;

namespace ECommerce.Catalog.WebAPI.Endpoints.Categories;

public static partial class EndpointsRegistration
{
    public static void RegisterCategoriesEndpoints(this WebApplication app)
    {
        var versionSet = app
            .NewApiVersionSet("CatalogAPIs")
            .ReportApiVersions()
            .Build();
        var version = new ApiVersion(1, 0);

        var group = app
            .MapGroup("/v{version:apiVersion}/categories")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(version)
            .RequireAuthorization(CatalogPolicies.IsAuthenticated);

        Categories.Map(group);
    }
}
