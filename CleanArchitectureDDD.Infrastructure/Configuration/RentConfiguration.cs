using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Shared;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

internal sealed class RentConfiguration: IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable("rents");
        builder.HasKey(r => r.Id);
        builder.OwnsOne(r => r.Price, priceBuilder =>
        {
            priceBuilder.Property(pb => pb.CurrencyType)
                .HasConversion(c => c.Code, code => CurrencyType.FromCode(code!));
        });
        builder.OwnsOne(r => r.Maintenance, maintenanceBuilder =>
        {
            maintenanceBuilder.Property(mb => mb.CurrencyType)
                .HasConversion(c => c.Code, code => CurrencyType.FromCode(code!));
        });
        builder.OwnsOne(r => r.AccessoriesPrice, accessoriesPriceBuilder =>
        {
            accessoriesPriceBuilder.Property(ab => ab.CurrencyType)
                .HasConversion(c => c.Code, code => CurrencyType.FromCode(code!));
        });
        builder.OwnsOne(r => r.TotalPrice, totalPriceBuilder =>
        {
            totalPriceBuilder.Property(tb => tb.CurrencyType)
                .HasConversion(c => c.Code, code => CurrencyType.FromCode(code!));
        });
        builder.OwnsOne(r => r.Duration);
        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(r => r.VehicleId);
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);
        
    }
}