using Bogus;
using CleanArchitectureDDD.Application.Abstractions.Data;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;
using CleanArchitectureDDD.Infrastructure.Contexts;
using Dapper;

namespace CleanArchitectureDDD.WebApi.Extensions;

public static class SeedDataExtensions
{
    public static void SeedAuthenticationData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var serviceProvider = serviceScope.ServiceProvider;
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Set<User>().Any())
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password123");
                var user = User.Create(
                    new Name("David"), 
                    new LastName("Jimenez"),
                    new Email("davidjimenez92@gmail.com"), 
                    new PasswordHash(passwordHash)
                    );
                context.AddAsync(user);
                
                var passwordHash2 = BCrypt.Net.BCrypt.HashPassword("Password123");
                user = User.Create(
                    new Name("Admin"), 
                    new LastName("Admin"),
                    new Email("admin@admin.com"), 
                    new PasswordHash(passwordHash2)
                );
                context.AddAsync(user);
                
                context.SaveChangesAsync().Wait();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
            logger.LogError(ex.Message);
        }
    }
    public static void SeedData(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var sqlConnectionFactory = serviceScope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker("es");

        List<object> vehicles = new();
        for (var i = 0; i < 100; i++)
        {
            vehicles.Add(new
            {
                Id = Guid.NewGuid(),
                Vin = faker.Vehicle.Vin(),
                Model = faker.Vehicle.Model(),
                Street = faker.Address.StreetAddress(),
                City = faker.Address.City(),
                State = faker.Address.StateAbbr(),
                ZipCode = faker.Address.ZipCode(),
                Price = Math.Round(faker.Random.Decimal(1000, 20000), 2),
                CurrencyType = "USD",
                PriceMaintenance = Math.Round(faker.Random.Decimal(100, 200), 2),
                CurrencyMaintenance = "USD",
                Accessories = new List<int>{ (int)Accessory.Wifi, (int)Accessory.AppleCarPlay },
                LastRent = DateTime.MinValue
            });
        }
        
        const string sql = """
                         INSERT INTO public.vehicles
                         (id, vin, model, address_street, address_city, address_state, address_zip_code, price_price, price_currency_type, maintenance_price, maintenance_currency_type, accessories, last_rent)
                         values(@Id, @Vin, @Model, @Street, @City, @State, @ZipCode, @Price, @CurrencyType, @PriceMaintenance, @CurrencyMaintenance, @Accessories, @LastRent)
                         """;
        connection.Execute(sql, vehicles);
    }
}