using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Tests.Application.Utilities;

namespace Tests.API.Controllers;

public class AdminControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly string _jwtSecret = "your-secure-secret-key";  // Should match the one in appsettings.json
    private readonly string _issuer = "your-app-name";  // Should match the one in appsettings.json
    private readonly string _audience = "your-app-audience";    // Should match the one in appsettings.json

    [Fact]
    public async Task GetAdminDashboard_WithValidAdminRole_ReturnsOk()
    {
        // Arrange
        var token = JwtTokenGenerator.GenerateToken(_jwtSecret, _issuer, _audience);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/v1/admin/dashboard");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminDashboard_WithoutJwt_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/admin/dashboard");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminDashboard_WithNonAdminRole_ReturnsForbidden()
    {
        // Arrange
        var token = JwtTokenGenerator.GenerateTokenWithRole(_jwtSecret, _issuer, _audience, "User");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/v1/admin/dashboard");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
