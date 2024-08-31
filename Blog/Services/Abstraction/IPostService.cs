using Blog.Dtos;
using Blog.Models.API_CORE;
using Blog.Models.Blog;

namespace Blog.Services.Abstraction;

public interface IPostService 
{
    Task<Post> CreatePostAsync(PostCreation postCreation);
    
    Task<ApiResponse<PaginatedResult<PostDto>>> GetPostsAsync(int pageNumber, int pageSize);
    
    // publish draft post
    Task<PostDto> PublishPostAsync(int postId);

}