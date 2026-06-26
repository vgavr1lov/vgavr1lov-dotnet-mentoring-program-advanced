using ECommerce.Cart.WebAPI.Authorization;

namespace ECommerce.Cart.WebAPI.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddCartAuthorization(this IServiceCollection services)
    {
        return services.AddAuthorization(options =>
            options.AddPolicy(CartPolicies.IsAuthenticated, policy => 
                policy.RequireAuthenticatedUser()));
    }
}
