using Blog.Dtos;
using Blog.Extensions;
using Blog.Models.Admin;

namespace Blog.Services.Abstraction;

public interface IAdminService
{
    Task<Admin> CreateAdminAsync(AdminCreation adminCreation);
    
    Task<Admin> GetAdminByEmailAsync(string email);
    
    // login
    Task<Token> LoginAdminAsync(AdminLogin adminLogin);
    
    // validate token
    Task<bool> ValidateAdminTokenAsync(string token);
    
}