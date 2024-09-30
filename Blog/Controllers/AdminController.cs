using Blog.Dtos;
using Blog.Extensions;
using Blog.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Blog;

[ApiController]
[Route("api/v1/[controller]")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] AdminCreation adminCreation)
    {
        var admin = await _adminService.CreateAdminAsync(adminCreation);
        
        return Ok(admin);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAdmin([FromBody] AdminLogin adminLogin)
    {
        var token = await _adminService.LoginAdminAsync(adminLogin);
        
        return Ok(token);
    }
    
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateAdminToken()
    {
        // Extract the token from the Authorization header
        if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return BadRequest("Authorization header is missing");
        }

        var token = authHeader.ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Token is missing or invalid");
        }

        var isValid = await _adminService.ValidateAdminTokenAsync(token);
    
        object response = new { isValid };
        return Ok(response);
    }
    
    
    
    
}