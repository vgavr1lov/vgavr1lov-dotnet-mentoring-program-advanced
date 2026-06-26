using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.Cart.WebAPI.Middleware;

public class TokenLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenLoggingMiddleware> _logger;

    public TokenLoggingMiddleware(RequestDelegate next, ILogger<TokenLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers["Authorization"].ToString();

        if (authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Replace("Bearer ", string.Empty);
            var handler = new JsonWebTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJsonWebToken(token);

                foreach (var item in jwtToken.Claims)
                {
                    _logger.LogInformation($"{item.Type}: {item.Value}");
                }
            }
        }

        await _next(context);
    }
}
