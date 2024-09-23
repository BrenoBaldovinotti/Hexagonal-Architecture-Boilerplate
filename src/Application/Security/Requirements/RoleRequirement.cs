using Microsoft.AspNetCore.Authorization;

namespace Application.Security.Requirements;

public class RoleRequirement(string requiredRole) : IAuthorizationRequirement
{
    public string RequiredRole { get; } = requiredRole;
}
