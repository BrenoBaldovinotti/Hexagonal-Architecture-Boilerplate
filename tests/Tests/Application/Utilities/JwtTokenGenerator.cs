using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Tests.Application.Utilities;

public static class JwtTokenGenerator
{
    public static string GenerateToken(string key, string issuer, string audience, int expireMinutes = 60)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, "test-user-id"),
                new Claim(JwtRegisteredClaimNames.Email, "test@example.com"),
                new Claim(ClaimTypes.Role, "Admin")            };

        var token = new JwtSecurityToken(issuer,
          audience,
          claims,
          expires: DateTime.Now.AddMinutes(expireMinutes),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateTokenWithRole(string key, string issuer, string audience, string role, int expireMinutes = 60)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, "test-user-id"),
        new Claim(JwtRegisteredClaimNames.Email, "test@example.com"),
        new Claim(ClaimTypes.Role, role)
    };

        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddMinutes(expireMinutes), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
