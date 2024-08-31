using System.Reflection;
using Blog.Data;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Extensions;

public static class ServiceRegistration
{
    public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
            services.AddControllers()
                .AddFluentValidation(V =>
                {
                    V.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });
        services.AddDbContext<DataContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        
        
        services.AddDependencyServices(configuration);
        services.AddAuthService(configuration);
    }
}