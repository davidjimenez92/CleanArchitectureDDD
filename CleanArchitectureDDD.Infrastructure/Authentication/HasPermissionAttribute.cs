using CleanArchitectureDDD.Domain.Entities.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitectureDDD.Infrastructure.Authentication;

public class HasPermissionAttribute: AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionEnum permission) : base(permission.ToString())
    {
        
    }
}