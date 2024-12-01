using Microsoft.AspNetCore.Authorization;

namespace CleanArchitectureDDD.Infrastructure.Authentication;

public class PermissionRequirement: IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}