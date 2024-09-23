using Application.Security.AuthorizationHandlers;
using Application.Security.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Tests.Application.Authorization;

public class RoleAuthorizationHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserWithAdminRole_Succeeds()
    {
        // Arrange
        var requirement = new RoleRequirement("Admin");
        var handler = new RoleAuthorizationHandler();

        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, "Admin")], "mock"));

        var context = new AuthorizationHandlerContext([requirement], user, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserWithoutAdminRole_Fails()
    {
        // Arrange
        var requirement = new RoleRequirement("Admin");
        var handler = new RoleAuthorizationHandler();

        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, "User")], "mock"));

        var context = new AuthorizationHandlerContext([requirement], user, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }
}
