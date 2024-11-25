using CleanArchitectureDDD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

internal sealed class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(user => user.Name)
            .HasMaxLength(200)
            .HasConversion(n => n!.Value, name => new Name(name));
        builder.Property(user => user.LastName)
            .HasMaxLength(200)
            .HasConversion(ln => ln!.Value, lastName => new LastName(lastName));
        builder.Property(user => user.Email)
            .HasMaxLength(200)
            .HasConversion(e => e!.Value, email => new Domain.Users.Email(email));
        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}