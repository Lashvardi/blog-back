namespace Blog.Services.Abstraction;

public interface ITokenService
{
    Task<string> GenerateAdminToken(string email);
    
    // validate token
    Task<bool> ValidateAdminTokenAsync(string token);
}