// Controllers/SurveyController.cs
// This controller demonstrates mixed protection on endpoints
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // DEFAULT: All endpoints in this controller require a valid token
public class SurveyController : ControllerBase
{
    // GET /api/survey/public
    // Overrides the controller-level [Authorize] — no token needed
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult GetPublicSurveys()
    {
        return Ok(new { Message = "These surveys are public — no token required!" });
    }

    // GET /api/survey/my-surveys
    // Inherits [Authorize] from the controller — valid token required
    [HttpGet("my-surveys")]
    public IActionResult GetMySurveys()
    {
        // HttpContext.User is populated by UseAuthentication() middleware
        // because we reached here with a valid token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst("sub")?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            Message = "You are authenticated!",
            UserId = userId,
            Email = email
        });
    }

    // GET /api/survey/admin-only
    // Only users with the "Admin" role claim can access this
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult GetAdminDashboard()
    {
        return Ok(new { Message = "Welcome, Admin! You have the Admin role claim." });
    }
}