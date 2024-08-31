using AutoMapper;
using Blog.Data;
using Blog.Dtos;
using Blog.Extensions;
using Blog.Models.API_CORE;
using Blog.Models.Blog;
using Blog.Models.Blog.Enums;
using Blog.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Implementation;

public class PostService: IPostService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public PostService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Post> CreatePostAsync(PostCreation postCreation)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == postCreation.CategoryId);
        var post = new Post
        {
            Title = postCreation.Title,
            Content = postCreation.Content,
            CreatedAt = DateTime.Now,
            CategoryId = postCreation.CategoryId,
            Category = category,
            Status = postCreation.isDraft ? BLOG_STATUS.DRAFT : BLOG_STATUS.PUBLISHED
        };
        
        post.PostTags = postCreation.TagIds.Select(tagId => new PostTag
        {
            Post = post,
            TagId = tagId
        }).ToList();
        
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        
        return post;
    }
    
    public async Task<ApiResponse<PaginatedResult<PostDto>>> GetPostsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("Invalid page number or page size.");
        }

        var totalPosts = await _context.Posts.Where(p => p.Status == BLOG_STATUS.PUBLISHED).CountAsync();

        if (totalPosts == 0)
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("No posts found.");
        }

        var posts = await _context.Posts
            .Include(p => p.Category)
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Where(p => p.Status == BLOG_STATUS.PUBLISHED)
            .ToListAsync();

        if (posts == null || !posts.Any())
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("No posts found for the specified page.");
        }
        
        var postDtos = _mapper.Map<List<PostDto>>(posts);
        
        foreach (var postDto in postDtos)
        {
            postDto.FormattedElapsedTimeSinceCreation = postDto.CreatedAt.ElapsedTimeSinceCreation();
        }
        
        var paginatedResult = new PaginatedResult<PostDto>(postDtos, totalPosts, pageNumber, pageSize);
        return ApiResponse<PaginatedResult<PostDto>>.SuccessResponse(paginatedResult, "Posts retrieved successfully");
    }
    
    public async Task<PostDto> PublishPostAsync(int postId)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post == null)
        {
            return null;
        }
        
        post.Status = BLOG_STATUS.PUBLISHED;
        await _context.SaveChangesAsync();
        
        return _mapper.Map<PostDto>(post);
    }

}