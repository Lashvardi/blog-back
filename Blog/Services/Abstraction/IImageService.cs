namespace Blog.Services.Abstraction;

public interface IImageService
{
    // upload base64 image
    Task<string> UploadBase64ImageAsync(string base64Image);
}