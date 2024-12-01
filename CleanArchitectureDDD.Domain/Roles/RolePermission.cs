using CleanArchitectureDDD.Domain.Entities.Permissions;

namespace CleanArchitectureDDD.Domain.Entities.Roles;

public sealed class RolePermission
{
    public int RoleId { get; set; }
    public PermissionId PermissionId { get; set; }
}