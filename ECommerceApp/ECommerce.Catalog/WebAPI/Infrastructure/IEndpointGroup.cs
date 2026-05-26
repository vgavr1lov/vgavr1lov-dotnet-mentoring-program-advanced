namespace ECommerce.Catalog.WebAPI.Infrastructure;

public interface IEndpointGroup
{
    static virtual string? RoutePrefix => null;
    static abstract void Map(RouteGroupBuilder groupBuilder);
}
