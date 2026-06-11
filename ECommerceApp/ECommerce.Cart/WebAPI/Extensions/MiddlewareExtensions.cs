using ECommerce.Cart.WebAPI.Middleware;

namespace ECommerce.Cart.WebAPI.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseTokenLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TokenLoggingMiddleware>();
    }
}
