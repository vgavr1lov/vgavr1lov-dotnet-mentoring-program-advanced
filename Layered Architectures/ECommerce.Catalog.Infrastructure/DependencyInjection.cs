using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Infrastructure.Data.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string dbPath)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
