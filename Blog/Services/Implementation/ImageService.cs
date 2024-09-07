using Blog.Services.Abstraction;

namespace Blog.Services.Implementation;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> UploadBase64ImageAsync(string base64Image)
    {
        if (string.IsNullOrEmpty(base64Image))
        {
            throw new ArgumentException("Base64 image string is null or empty.", nameof(base64Image));
        }

        // Remove the data:image/png;base64, part if it exists
        var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

        // Convert base64 string to byte array
        byte[] imageBytes = Convert.FromBase64String(base64Data);

        // Generate a unique filename
        string fileName = $"{Guid.NewGuid()}.png";

        // Combine the uploads folder path with the filename
        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        string filePath = Path.Combine(uploadsFolder, fileName);

        // Ensure the uploads folder exists
        Directory.CreateDirectory(uploadsFolder);

        // Save the file
        await File.WriteAllBytesAsync(filePath, imageBytes);

        // Generate the URL for the saved image
        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.PathBase}";
        var imageUrl = $"{baseUrl}/uploads/{fileName}";

        return imageUrl;
    }
}
