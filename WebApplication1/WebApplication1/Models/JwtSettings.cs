// Models/JwtSettings.cs
namespace WebApplication1.Models;

public class JwtSettings
{
    // These property names must exactly match the keys in appsettings.json
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 60;
}