using Blog.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Blog;
[ApiController]
[Route("api/v1/[controller]")]
public class ImageController : Controller
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadBase64Image([FromBody] string base64Image)
    {
        try
        {
            var imageUrl = await _imageService.UploadBase64ImageAsync(base64Image);
            return Ok(imageUrl);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
}