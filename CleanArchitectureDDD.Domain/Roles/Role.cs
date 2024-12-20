using CleanArchitectureDDD.Domain.Entities.Permissions;
using CleanArchitectureDDD.Domain.Shared;

namespace CleanArchitectureDDD.Domain.Entities.Roles;

public sealed class Role: Enumeration<Role>
{
    public static readonly Role Client = new(1, "Client");
    public static readonly Role Admin = new(2, "Admin");
    public Role(int id, string name) : base(id, name)
    {
    }
    
    public ICollection<Permission>? Permissions { get; set; }
}