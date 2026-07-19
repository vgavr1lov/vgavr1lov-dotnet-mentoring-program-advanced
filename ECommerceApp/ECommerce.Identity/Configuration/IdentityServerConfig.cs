using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using static ECommerce.Identity.Configuration.SecurityConstants;

namespace ECommerce.Identity.Configuration;

public static class IdentityServerConfig
{
    public static List<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new(Scopes.RolesScope, "User roles", [SecurityConstants.ClaimTypes.Role]),
        };
    }

    public static List<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>()
        {
            new(ApiResources.ECommerceApi),
            new(Permissions.CatalogRead),
            new(Permissions.CatalogCreate),
            new(Permissions.CatalogUpdate),
            new(Permissions.CatalogDelete),
        };
    }

    public static List<ApiResource> GetApiResources()
    {
        var resource = new ApiResource(ApiResources.ECommerceApi, "ECommerce API")
        {
            Scopes = {
                ApiResources.ECommerceApi,
                Permissions.CatalogRead,
                Permissions.CatalogCreate,
                Permissions.CatalogUpdate,
                Permissions.CatalogDelete, },
            UserClaims = {
                SecurityConstants.ClaimTypes.Role,
                SecurityConstants.ClaimTypes.Permission, },
        };

        return new List<ApiResource> { resource };
    }

    public static List<Client> GetClients(IConfiguration configuration)
    {
        var redirectUris = configuration
            .GetSection("IdentityServer:Clients:Swagger:RedirectUris")
            .Get<List<string>>() ?? [];

        var corsOrigins = configuration
            .GetSection("IdentityServer:Clients:Swagger:CorsOrigins")
            .Get<List<string>>() ?? [];

        return new List<Client>
        {
            new() {
                ClientId = "postman",
                ClientSecrets =
                {
                    new Secret("postman".Sha256()),
                },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RedirectUris = redirectUris,
                AllowedCorsOrigins = corsOrigins,
                AllowedScopes = [
                    Scopes.OpenId,
                    Scopes.Profile,
                    Scopes.RolesScope,
                    Scopes.OfflineAccess,
                    Permissions.CatalogRead,
                    Permissions.CatalogCreate,
                    Permissions.CatalogUpdate,
                    Permissions.CatalogDelete],
                AccessTokenLifetime = AccessTokenLifetimeInSeconds,
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
            },
            new() {
                    ClientId = "swagger",
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = redirectUris,
                    AllowedCorsOrigins = corsOrigins,
                    AllowedScopes = [
                        Scopes.OpenId,
                        Scopes.Profile,
                        Scopes.RolesScope,
                        Scopes.OfflineAccess,
                        Permissions.CatalogRead,
                        Permissions.CatalogCreate,
                        Permissions.CatalogUpdate,
                        Permissions.CatalogDelete,
                        ApiResources.ECommerceApi],
                    AccessTokenLifetime = AccessTokenLifetimeInSeconds,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                },
        };
    }

    public static List<TestUser> GetTestUsers()
    {
        return new List<TestUser>
        {
            new() {
                SubjectId = "1",
                Username = "manager",
                Password = "manager",
                Claims = new List<Claim>
                {
                    new(SecurityConstants.ClaimTypes.Role, Roles.Manager),
                },
            },
            new() {
                SubjectId = "2",
                Username = "customer",
                Password = "customer",
                Claims = new List<Claim>
                {
                    new(SecurityConstants.ClaimTypes.Role, Roles.StoreCustomer),
                },
            },
        };
    }
}
