namespace ECommerce.Catalog.WebAPI.Configuration;

public static class SecurityConstants
{
    public static class ClaimTypes
    {
        public const string Permission = "permission";
    }

    public static class Permissions
    {
        public const string CatalogRead = "catalog.read";
        public const string CatalogCreate = "catalog.create";
        public const string CatalogUpdate = "catalog.update";
        public const string CatalogDelete = "catalog.delete";
    }
}
