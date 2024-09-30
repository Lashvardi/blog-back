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
    
    
    [HttpPost("initialize-post")]
    public async Task<IActionResult> InitializePostAsync()
    {
        var post = await _postService.InitializePostAsync();
        
        return Ok(post);
    }
    
    // partially save post details
    [HttpPut("partially-save-post")]
    public async Task<IActionResult> PartiallySavePostAsync([FromBody] PostCreation postCreation, int postId)
    {
        var post = await _postService.PartiallySavePostAsync(postCreation, postId);
        
        return Ok(post);
    }
    
    
    [HttpPost("create-post")]
    public async Task<IActionResult> CreatePostAsync([FromBody] PostCreation postCreation)
    {
        var post = await _postService.CreatePostAsync(postCreation);
        
        return Ok(post);
    }
    
    [HttpPost("assign-cover-image")]
    public async Task<IActionResult> AssignCoverImageAsync(int postId, string imageUrl)
    {
        var post = await _postService.AssignCoverImageAsync(postId, imageUrl);
        
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
    
    [HttpGet("get-all-posts")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<PostDto>>>> GetAllPosts(int pageNumber = 1, int pageSize = 10)
    {
        var paginatedResult = await _postService.GetAllPostsAsync(pageNumber, pageSize);
        
        return Ok(paginatedResult);
    }
    
    [HttpGet("get-post/{postId}")]
    public async Task<IActionResult> GetPostByIdAsync(int postId)
    {
        var post = await _postService.GetPostByIdAsync(postId);
        
        return Ok(post);
    }
    
    // get post tags
    [HttpGet("get-post-tags/{postId}")]
    public async Task<IActionResult> GetPostTags(int postId)
    {
        var postTags = await _postService.GetPostTagNames(postId);
        
        return Ok(postTags);
    }
    
    
    [HttpDelete("delete-post/{postId}")]
    public async Task<IActionResult> DeletePostAsync(int postId)
    {
        var post = await _postService.DeletePostAsync(postId);
        
        return Ok(post);
    }
    
    [HttpGet("get-all-posts-no-content")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>>> GetAllPostsNoContent(int pageNumber = 1, int pageSize = 10)
    {
        var paginatedResult = await _postService.GetAllPostsAsyncNoContent(pageNumber, pageSize);
        
        return Ok(paginatedResult);
    }
    
    [HttpGet("get-all-posts-no-content-10")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>>> GetAllPostsNoContent()
    {
        var paginatedResult = await _postService.GetAllPostsAsyncNoContent();
        
        return Ok(paginatedResult);
    }
    
    
    [HttpGet("get-suggested-posts/{postId}")]
    public async Task<IActionResult> Get3SuggestedPosts(int postId)
    {
        var suggestedPosts = await _postService.Get3SuggestedPosts(postId);
        
        return Ok(suggestedPosts);
    }
    
    // seed posts
    [HttpPost("seed-posts")]
    public async Task<IActionResult> SeedPosts()
    {

        _postService.SeedPostsAsync();
        
        return Ok();
    }
    
    // search posts
    [HttpGet("search-posts")]
    public async Task<ActionResult<ApiResponse<PaginatedResult<PostDto>>>>
        SearchPosts(string query, int pageNumber = 1, int pageSize = 10)
    {
        var paginatedResult = await _postService.SearchPostsAsync(query, pageNumber, pageSize);
        
        return Ok(paginatedResult);
    }
}