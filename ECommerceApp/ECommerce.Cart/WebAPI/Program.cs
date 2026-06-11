using ECommerce.Cart.WebAPI;
using ECommerce.Cart.WebAPI.Endpoints.Carts;
using ECommerce.Cart.WebAPI.Extensions;
using ECommerce.Cart.WebAPI.Infrastructure;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (!string.Equals(builder.Environment.EnvironmentName, "Testing", StringComparison.OrdinalIgnoreCase))
            await builder.Services.AddJwtBearerAuthentication(builder.Configuration);

        builder.Services.AddCartAuthorization();

        builder.Services.AddWebAPIServices(builder.Configuration);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerConfiguration(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.UseSwaggerConfiguration();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseTokenLogging();
        app.UseAuthorization();

        app.RegisterCartsEndpoints();

        app.HandleExceptions();

        app.Run();
    }
}