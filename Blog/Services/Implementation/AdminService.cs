using Blog.Data;
using Blog.Dtos;
using Blog.Exceptions;
using Blog.Extensions;
using Blog.Models.Admin;
using Blog.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Implementation;

public class AdminService : IAdminService
{

    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    
    
    public AdminService(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    public async Task<Admin> CreateAdminAsync(AdminCreation adminCreation)
    {
        var admin = new Admin
        {
            Email = adminCreation.Email,
            FullName = adminCreation.FullName,
            Password = BCrypt.Net.BCrypt.HashPassword(adminCreation.Password)
        };
        
        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
        
        return admin;
    }
    
    public async Task<Admin> GetAdminByEmailAsync(string email)
    {
        return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
    }
    
    public async Task<Token> LoginAdminAsync(AdminLogin adminLogin)
    {
        var admin = await GetAdminByEmailAsync(adminLogin.Email);
        
        if (admin == null)
        {
            throw new NotFoundException("Admin not found");
        }
        
        if (!BCrypt.Net.BCrypt.Verify(adminLogin.Password, admin.Password))
        {
            throw new UnauthorizedException("Invalid credentials");
        }
        
        var token = await _tokenService.GenerateAdminToken(admin.Email);
        
        return new Token
        {
            Value = token
        };
    }
    
    

}