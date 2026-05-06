using Asp.Versioning.Builder;

namespace ECommerce.Cart.WebAPI.Infrastructure;

public interface IEndpointGroup
{
    static virtual string? RoutePrefix => null;
    static abstract void Map(RouteGroupBuilder groupBuilder);
}
