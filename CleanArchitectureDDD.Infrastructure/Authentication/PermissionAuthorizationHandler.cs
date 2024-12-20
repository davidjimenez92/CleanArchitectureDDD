using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitectureDDD.Infrastructure.Authentication;

public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequirement>
{
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return Task.CompletedTask;
        }

        HashSet<string> permissions = context.User.Claims
            .Where(c => c.Type == CustomClaims.Permissions)
            .Select(x => x.Value)
            .ToHashSet();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}