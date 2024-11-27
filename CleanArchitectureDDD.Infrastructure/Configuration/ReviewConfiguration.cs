using CleanArchitectureDDD.Domain.Rentals;
using CleanArchitectureDDD.Domain.Reviews;
using CleanArchitectureDDD.Domain.Users;
using CleanArchitectureDDD.Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

internal sealed class ReviewConfiguration: IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasConversion(rId => rId!.Value, value => new ReviewId(value));
        builder.Property(r => r.Rating)
            .HasConversion(r => r!.Value, v => Rating.Create(v).Value);
        builder.Property(r => r.Comment)
            .HasMaxLength(200)
            .HasConversion(r => r!.Value, value => new Comment(value));
        builder.HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(r => r.VehicleId);
        builder.HasOne<Rental>()
            .WithMany()
            .HasForeignKey(r => r.RentalId);
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}