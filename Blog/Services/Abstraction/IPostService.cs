using Blog.Dtos;
using Blog.Models.API_CORE;
using Blog.Models.Blog;

namespace Blog.Services.Abstraction;

public interface IPostService 
{
    Task<Post> CreatePostAsync(PostCreation postCreation);
    
    Task<ApiResponse<PaginatedResult<PostDto>>> GetPostsAsync(int pageNumber, int pageSize);
    
    Task<ApiResponse<PaginatedResult<PostDto>>> GetAllPostsAsync(int pageNumber, int pageSize);

    // publish draft post
    Task<PostDto> PublishPostAsync(int postId);
    
    // initialize post
    Task<Post> InitializePostAsync();
    
    // partially save post details
    Task<Post> PartiallySavePostAsync(PostCreation postCreation, int postId);
    
    // assign cover image to post
    Task<Post> AssignCoverImageAsync(int postId, string imageUrl);
    
    // get post by id
    Task<Post> GetPostByIdAsync(int postId);
    
    // delete post
    Task<Post> DeletePostAsync(int postId);
    

}