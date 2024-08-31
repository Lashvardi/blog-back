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
    
    
    
    
}