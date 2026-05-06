using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerce.Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly));

        var loggerFactory = LoggerFactory.Create(builder => { });
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, loggerFactory);
        IMapper mapper = mapperConfig.CreateMapper();

        services.AddSingleton(mapper);

        return services;
    }
}
