using CleanArchitectureDDD.Application.Abstractions.Clock;
using CleanArchitectureDDD.Application.Abstractions.Email;
using CleanArchitectureDDD.Infrastructure.Clock;
using CleanArchitectureDDD.Infrastructure.Contexts;
using CleanArchitectureDDD.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

public static class IoC
{
    public static IServiceCollection AddInfrastructureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new ArgumentNullException(nameof(configuration));
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });
        return services;
    }
}