using ECommerce.Cart.WebAPI;
using ECommerce.Cart.WebAPI.Endpoints.Carts;
using ECommerce.Cart.WebAPI.Infrastructure;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddWebAPIServices(builder.Configuration);
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            //app.UseSwaggerUI(options =>
            //{
            //    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            //    foreach (var description in provider.ApiVersionDescriptions)
            //    {
            //        options.SwaggerEndpoint(
            //            $"/swagger/{description.GroupName}/swagger.json",
            //            description.GroupName.ToUpperInvariant());
            //    }
            //    //options.DefaultModelsExpandDepth(-1);
            //});
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
                options.DefaultModelsExpandDepth(-1);
            });
        }

        app.RegisterCartsEndpoints();

        app.UseHttpsRedirection();

        app.HandleExceptions();

        app.Run();
    }
}