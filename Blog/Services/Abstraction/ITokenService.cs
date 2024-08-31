namespace Blog.Services.Abstraction;

public interface ITokenService
{
    Task<string> GenerateAdminToken(string email);
}