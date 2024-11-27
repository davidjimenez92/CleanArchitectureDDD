using CleanArchitectureDDD.Application.Abstractions.Behaviors;
using CleanArchitectureDDD.Domain.Rentals;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDDD.Application.Configuration
{
    public static class IoC
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(typeof(IoC).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(IoC).Assembly);
            services.AddTransient<PriceService>();
            
            return services;
        }
    }
}