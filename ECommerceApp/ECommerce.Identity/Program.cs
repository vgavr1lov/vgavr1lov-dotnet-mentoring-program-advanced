using ECommerce.Identity.Configuration;
using ECommerce.Identity.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options => options.IssuerUri = builder.Configuration["IdentityServer:IssuerUri"])
            .AddDeveloperSigningCredential(persistKey: false)
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            .AddTestUsers(IdentityServerConfig.GetTestUsers())
            .AddInMemoryClients(IdentityServerConfig.GetClients(builder.Configuration))
            .AddProfileService<ProfileService>();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                var corsOrigins = builder.Configuration
                    .GetSection("IdentityServer:Clients:Swagger:CorsOrigins")
                    .Get<string[]>() ?? [];

                policy.WithOrigins(corsOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        app.UseCors();
        app.UseStaticFiles();
        app.UseIdentityServer();
        app.MapRazorPages();

        app.Run();
    }
}
