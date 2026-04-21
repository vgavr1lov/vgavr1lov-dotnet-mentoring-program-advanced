using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Catalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
