using Asp.Versioning;
using ECommerce.Cart.WebAPI.Authorization;

namespace ECommerce.Cart.WebAPI.Endpoints.Carts;

public static partial class EndpointsRegistration
{
    public static void RegisterCartsEndpoints(this WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(new ApiVersion(1, 0))
                    .HasApiVersion(new ApiVersion(2, 0))
                    .ReportApiVersions()
                    .Build();

        var groupV1 = app
            .MapGroup("/v{version:apiVersion}/carts")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(new ApiVersion(1, 0))
            .RequireAuthorization(CartPolicies.IsAuthenticated);

        CartsV1.Map(groupV1);

        var groupV2 = app
            .MapGroup("/v{version:apiVersion}/carts")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(new ApiVersion(2, 0))
            .RequireAuthorization(CartPolicies.IsAuthenticated);

        CartsV2.Map(groupV2);
    }
}
