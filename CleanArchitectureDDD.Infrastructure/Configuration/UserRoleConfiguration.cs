using CleanArchitectureDDD.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_role");
        builder.HasKey(x => new { x.UserId, x.RoleId });
        builder.Property(x => x.UserId)
            .HasConversion(u => u!.Value, value => new UserId(value));
    }
}