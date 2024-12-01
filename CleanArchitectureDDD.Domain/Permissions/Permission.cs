using CleanArchitectureDDD.Domain.Abstractions;

namespace CleanArchitectureDDD.Domain.Entities.Permissions;

public sealed class Permission: Entity<PermissionId>
{
    private Permission()
    {
    }

    public Permission(PermissionId id, PermissionName? name): base(id)
    {
        Name = name;
    }

    public Permission(PermissionName? name): base()
    {
        Name = name;
    }
    
    public PermissionName? Name { get; init; }

    public static Result<Permission> Create(PermissionName name)
    {
        return new Permission(name);
    }
}