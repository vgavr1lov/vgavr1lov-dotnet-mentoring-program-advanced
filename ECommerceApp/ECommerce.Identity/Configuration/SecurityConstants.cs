namespace ECommerce.Identity.Configuration;

public static class SecurityConstants
{
    public const int AccessTokenLifetimeInSeconds = 3600;

    public static class Scopes
    {
        public const string OpenId = "openid";
        public const string Profile = "profile";
        public const string RolesScope = "roles";
        public const string OfflineAccess = "offline_access";
    }

    public static class ApiResources
    {
        public const string ECommerceApi = "ecommerce.api";
    }

    public static class ClaimTypes
    {
        public const string Role = "role";
        public const string Permission = "permission";
    }

    public static class Permissions
    {
        public const string CatalogRead = "catalog.read";
        public const string CatalogCreate = "catalog.create";
        public const string CatalogUpdate = "catalog.update";
        public const string CatalogDelete = "catalog.delete";
    }

    public static class Roles
    {
        public const string Manager = "Manager";
        public const string StoreCustomer = "StoreCustomer";
    }
}
