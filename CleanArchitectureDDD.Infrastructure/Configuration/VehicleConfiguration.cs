using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

internal sealed class VehicleConfiguration: IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(v => v.Id);
        builder.OwnsOne(v => v.Address);
        builder.Property(v => v.Model)
            .HasMaxLength(200)
            .HasConversion(m => m!.Value, n => new Model(n));
        builder.Property(v => v.Vin)
            .HasMaxLength(17)
            .HasConversion(v => v!.Value, v => new Vin(v));
        builder.OwnsOne(v => v.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.currencyType)
                .HasConversion(c => c.Code, code => CurrencyType.FromCode(code!));
        });
        builder.OwnsOne(v => v.Maintenance, maintenanceBuilder =>
        {
            maintenanceBuilder.Property(m => m.currencyType)
                .HasConversion(c => c.Code, code => CurrencyType.FromCode(code!));
        });
    }
}