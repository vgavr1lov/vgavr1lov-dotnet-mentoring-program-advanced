using ECommerce.Catalog.WebAPI;
using ECommerce.Catalog.WebAPI.Endpoints.Categories;
using ECommerce.Catalog.WebAPI.Endpoints.Products;
using ECommerce.Catalog.WebAPI.Infrastructure;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddWebAPIServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.RegisterCategoriesEndpoints();
        app.RegisterProductsEndpoints();

        app.HandleExceptions();

        app.Run();
    }
}