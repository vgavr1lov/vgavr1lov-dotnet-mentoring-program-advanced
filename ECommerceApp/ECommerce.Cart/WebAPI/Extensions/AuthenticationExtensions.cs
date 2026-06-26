using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Cart.WebAPI.Extensions;

public static class AuthenticationExtensions
{
    private static readonly HttpClient Http = new();

    public static async Task<IServiceCollection> AddJwtBearerAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        IdentityModelEventSource.ShowPII = true;

        var authority = configuration["IdentityServer:Authority"]!;
        var audience = configuration["IdentityServer:Audience"]!;
        var signingKeys = await GetSigningKeys(authority);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authority,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = signingKeys
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = async ctx =>
                {
                    if (ctx.Exception is SecurityTokenSignatureKeyNotFoundException)
                    {
                        ctx.Options.TokenValidationParameters.IssuerSigningKeys =
                            await GetSigningKeys(authority);
                    }
                }
            };
        });

        return services;
    }

    private static async Task<IList<SecurityKey>> GetSigningKeys(string authority)
    {
        var jwksJson = await Http.GetStringAsync(
            $"{authority}/.well-known/openid-configuration/jwks");
        return new JsonWebKeySet(jwksJson).GetSigningKeys();
    }
}