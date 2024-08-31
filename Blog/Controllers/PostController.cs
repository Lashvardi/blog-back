using Blog.Dtos;
using Blog.Models.API_CORE;
using Blog.Models.Blog;
using Blog.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Blog;

[ApiController]
[Route("api/v1/[controller]")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    
    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    [HttpPost("create-post")]
    public async Task<IActionResult> CreatePostAsync([FromBody] PostCreation postCreation)
    {
        var post = await _postService.CreatePostAsync(postCreation);
        
        return Ok(post);
    }
    
    [HttpPost("publish-draft-post")]
    public async Task<IActionResult> PublishPostAsync(int postId)
    {
        var post = await _postService.PublishPostAsync(postId);
        
        return Ok(post);
    }
    
    [HttpGet("get-posts")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<PostDto>>>> GetPosts(int pageNumber = 1, int pageSize = 10)
    {
        var paginatedResult = await _postService.GetPostsAsync(pageNumber, pageSize);
        
        return Ok(paginatedResult);
    }
}