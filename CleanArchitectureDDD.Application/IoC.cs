using CleanArchitectureDDD.Domain.Rentals;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDDD.Application;

public static class IoC
{
    public static IServiceCollection AddApplicationServices(IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(typeof(IoC).Assembly);
        });
        services.AddTransient<PriceService>();
        
        return services;
    }
}