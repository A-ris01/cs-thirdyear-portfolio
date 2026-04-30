// Services/TokenService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Services;

public interface ITokenService
{
    string GenerateToken(string userId, string email, string role);
}

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    // IOptions<T> is how ASP.NET Core injects configuration sections
    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(string userId, string email, string role)
    {
        // Step A: Create the signing key from our secret string
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
        );

        // Step B: Define the signing algorithm
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Step C: Define the Claims — the "payload" data inside the token.
        // Anyone can READ claims (they're Base64 encoded, not encrypted).
        // But no one can FORGE them without the secret key.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),      // Subject (user ID)
            new Claim(JwtRegisteredClaimNames.Email, email),     // Email
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            new Claim(ClaimTypes.Role, role),                    // Role for [Authorize(Roles="...")]
            new Claim("custom_claim", "any_value_you_want")      // You can add any custom data
        };

        // Step D: Build the token descriptor
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        // Step E: Serialize it to the final "eyJ..." string format
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}