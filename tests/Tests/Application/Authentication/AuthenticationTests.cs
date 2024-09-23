using API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using Tests.Application.Utilities;

namespace Tests.Application.Authentication;

public class AuthenticationTests(WebApplicationFactory<Startup> factory) : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Test_AuthenticatedRequest_WithValidJwt_ReturnsSuccess()
    {
        // Arrange
        var token = JwtTokenGenerator.GenerateToken("your-very-very-ginormous-very-strong-secret-key", "your-app-name", "your-app-audience");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/v1/admin/dashboard");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Test_UnauthenticatedRequest_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/admin/dashboard");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
