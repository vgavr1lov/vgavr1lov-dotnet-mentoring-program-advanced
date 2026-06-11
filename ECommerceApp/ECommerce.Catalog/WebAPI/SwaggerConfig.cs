using ECommerce.Catalog.WebAPI.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ECommerce.Catalog.WebAPI;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var authorizeUrl = configuration["IdentityServer:AuthorizeUrl"] ?? string.Empty;
        var tokenUrl = configuration["IdentityServer:TokenUrl"] ?? string.Empty;

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authorizeUrl),
                        TokenUrl = new Uri(tokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                                { "ecommerce.api", "ECommerce API Access" }
                        }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "ecommerce.api", "ECommerce API Access" }
                }
            });
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }

    public static WebApplication UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId("swagger");
            options.OAuthUsePkce();
        });
        return app;
    }
}
