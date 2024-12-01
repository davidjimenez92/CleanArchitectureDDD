using CleanArchitectureDDD.Domain.Entities.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

public sealed class PermissionConfiguration: IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, id => new PermissionId(id));
        
        builder.Property(x => x.Name)
            .HasConversion(name => name.Value, name => new PermissionName(name));

        IEnumerable<Permission> permissions = Enum.GetValues<PermissionEnum>()
            .Select(p => new Permission(new PermissionId((int)p), new PermissionName(p.ToString())));
        builder.HasData(permissions);
    }
}