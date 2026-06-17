using ECommerce.Catalog.WebAPI;
using ECommerce.Catalog.WebAPI.Endpoints.Categories;
using ECommerce.Catalog.WebAPI.Endpoints.Products;
using ECommerce.Catalog.WebAPI.Extensions;
using ECommerce.Catalog.WebAPI.Infrastructure;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfiguration(builder.Configuration);

        builder.Services.AddWebAPIServices(builder.Configuration);

        if (!string.Equals(builder.Environment.EnvironmentName, "Testing", StringComparison.OrdinalIgnoreCase))
            await builder.Services.AddJwtBearerAuthentication(builder.Configuration);

        builder.Services.AddCatalogAuthorization();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.UseSwaggerConfiguration();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.RegisterCategoriesEndpoints();
        app.RegisterProductsEndpoints();

        app.HandleExceptions();

        app.Run();
    }
}
