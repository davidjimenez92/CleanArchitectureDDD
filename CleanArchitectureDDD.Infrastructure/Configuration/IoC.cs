
using CleanArchitectureDDD.Application.Abstractions.Clock;
using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Application.Abstractions.Email;
using CleanArchitectureDDD.Domain.Abstractions;
using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Infrastructure.Clock;
using CleanArchitectureDDD.Infrastructure.Contexts;
using CleanArchitectureDDD.Infrastructure.Data;
using CleanArchitectureDDD.Infrastructure.Email;
using CleanArchitectureDDD.Infrastructure.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

public static class IoC
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();
        var connectionString = configuration.GetConnectionString("ConnectionString")
            ?? throw new ArgumentNullException(nameof(configuration));
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        
        return services;
    }
}