using Blog.Services.Abstraction;
using Blog.Services.Implementation;

namespace Blog.Extensions;

public static class DependencyServiceRegistration
{
    public static void AddDependencyServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(Program).Assembly);

    }
}