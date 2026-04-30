// Controllers/AuthController.cs
// This controller handles login and returns a token
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    // POST /api/auth/login
    // [AllowAnonymous] = No token needed. Anyone can call this.
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // ⚠️ In a real app: query the database and verify the password hash.
        // This is a hardcoded demo — DO NOT do this in production.
        if (request.Email == "admin@demo.com" && request.Password == "password123")
        {
            var token = _tokenService.GenerateToken(
                userId: "user-001",
                email: request.Email,
                role: "Admin"
            );

            return Ok(new { Token = token, Message = "Login successful" });
        }

        return Unauthorized(new { Message = "Invalid email or password" });
    }
}

// Simple DTO for the login request body
public record LoginRequest(string Email, string Password);