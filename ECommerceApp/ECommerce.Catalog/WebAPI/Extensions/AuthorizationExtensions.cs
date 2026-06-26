using ECommerce.Catalog.WebAPI.Authorization;
using ECommerce.Catalog.WebAPI.Configuration;

namespace ECommerce.Catalog.WebAPI.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCatalogAuthorization(this IServiceCollection services)
    {
        return services.AddAuthorization(options =>
        {
            options.AddPolicy(CatalogPolicies.CanRead, policy =>
                policy.RequireClaim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogRead));
            options.AddPolicy(CatalogPolicies.CanCreate, policy =>
                policy.RequireClaim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogCreate));
            options.AddPolicy(CatalogPolicies.CanUpdate, policy =>
                policy.RequireClaim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogUpdate));
            options.AddPolicy(CatalogPolicies.CanDelete, policy =>
                policy.RequireClaim(SecurityConstants.ClaimTypes.Permission, SecurityConstants.Permissions.CatalogDelete));
            options.AddPolicy(CatalogPolicies.IsAuthenticated, policy =>
                policy.RequireAuthenticatedUser());
        });
    }
}
