using CleanArchitectureDDD.Domain.Entities.Permissions;
using CleanArchitectureDDD.Domain.Entities.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureDDD.Infrastructure.Configuration;

public class RolePermissionConfiguration: IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.HasKey(x => new { x.RoleId, x.PermissionId });
        builder.Property(x => x.PermissionId)
            .HasConversion(pid => pid!.Value, value => new PermissionId(value));

        builder.HasData(
            CreateRolePermission(Role.Client, PermissionEnum.ReadUser),
            CreateRolePermission(Role.Admin, PermissionEnum.ReadUser),
            CreateRolePermission(Role.Admin, PermissionEnum.WriteUser),
            CreateRolePermission(Role.Admin, PermissionEnum.UpdateUser)
        );
    }

    private static RolePermission CreateRolePermission(Role role, PermissionEnum permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = new PermissionId((int)permission),
        };
    }
}