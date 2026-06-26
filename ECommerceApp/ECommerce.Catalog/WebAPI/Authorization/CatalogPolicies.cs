namespace ECommerce.Catalog.WebAPI.Authorization;

public static class CatalogPolicies
{
    public const string IsAuthenticated = "IsAuthenticated";

    public const string CanRead = "CanRead";
    public const string CanCreate = "CanCreate";
    public const string CanUpdate = "CanUpdate";
    public const string CanDelete = "CanDelete";
}
