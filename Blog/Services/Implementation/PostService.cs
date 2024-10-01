using AutoMapper;
using Blog.Data;
using Blog.Dtos;
using Blog.Exceptions;
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
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public PostService(DataContext context, IMapper mapper, IServiceScopeFactory serviceScopeFactory)
    {
        _context = context;
        _mapper = mapper;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task<Post> CreatePostAsync(PostCreation postCreation)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == postCreation.CategoryId);
        var post = new Post
        {
            Title = postCreation.Title,
            Content = postCreation.Content,
            Description = postCreation.Description,
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
    
    public async Task<ApiResponse<PaginatedResult<PostDto>>> GetAllPostsAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("Invalid page number or page size.");
        }

        var totalPosts = await _context.Posts.Where(p => p.Status != BLOG_STATUS.DELETED).CountAsync();

        if (totalPosts == 0)
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("No posts found.");
        }

        var posts = await _context.Posts
            .Include(p => p.Category)
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedAt)
            .Where(p => p.Status != BLOG_STATUS.DELETED)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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
    
    public async Task<ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>>  GetAllPostsAsyncNoContent(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.FailureResponse("Invalid page number or page size.");
        }

        var totalPosts = await _context.Posts.Where(p => p.Status != BLOG_STATUS.DELETED).CountAsync();

        if (totalPosts == 0)
        {
            return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.FailureResponse("No posts found.");
        }

        var posts = await _context.Posts
            .Include(p => p.Category)
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedAt)
            .Where(p => p.Status == BLOG_STATUS.PUBLISHED)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (posts == null || !posts.Any())
        {
            return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.FailureResponse("No posts found for the specified page.");
        }
        
        var postDtos = _mapper.Map<List<GetPostDtoWithoutContent>>(posts);
        
        foreach (var postDto in postDtos)
        {
            postDto.FormattedElapsedTimeSinceCreation = postDto.CreatedAt.ElapsedTimeSinceCreation();
        }
        
        var paginatedResult = new PaginatedResult<GetPostDtoWithoutContent>(postDtos, totalPosts, pageNumber, pageSize);
        return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.SuccessResponse(paginatedResult, "Posts retrieved successfully");
    }
    
    // same as above but for 10 posts
    public async Task<ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>>  GetAllPostsAsyncNoContent()
    {
        var totalPosts = await _context.Posts.Where(p => p.Status != BLOG_STATUS.DELETED).CountAsync();

        if (totalPosts == 0)
        {
            return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.FailureResponse("No posts found.");
        }

        var posts = await _context.Posts
            .Include(p => p.Category)
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedAt)
            .OrderByDescending(p => p.CreatedAt)
            .Where(p => p.Status == BLOG_STATUS.PUBLISHED)
            .Take(10)   
            .ToListAsync();

        if (posts == null || !posts.Any())
        {
            return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.FailureResponse("No posts found for the specified page.");
        }
        
        var postDtos = _mapper.Map<List<GetPostDtoWithoutContent>>(posts);
        
        foreach (var postDto in postDtos)
        {
            postDto.FormattedElapsedTimeSinceCreation = postDto.CreatedAt.ElapsedTimeSinceCreation();
        }
        
        
        var paginatedResult = new PaginatedResult<GetPostDtoWithoutContent>(postDtos, totalPosts, 0,10);
        return ApiResponse<PaginatedResult<GetPostDtoWithoutContent>>.SuccessResponse(paginatedResult, "Posts retrieved successfully");
    }
    
    public async Task<List<PostTagName>> GetPostTagNames(int postId)
    {
        var postTagNames = await _context.PostTags
            .Include(pt => pt.Tag)
            .Where(pt => pt.PostId == postId)
            .Select(pt => new PostTagName
            {
                Id = pt.TagId,
                Name = pt.Tag.Name
            })
            .ToListAsync();

        return postTagNames;
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
    
    public async Task<Post> InitializePostAsync()
    {
        var post = new Post
        {
            CreatedAt = DateTime.Now,
            Status = BLOG_STATUS.DRAFT
        };
        
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        
        return post;
    }
    
    public async Task<Post> PartiallySavePostAsync(PostCreation postCreation, int postId)
    {
        // Step 1: Load the post with its tags
        var post = await _context.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return null;
        }

        // Step 2: Update the post fields
        post.Title = postCreation.Title;
        post.Content = postCreation.Content;
        post.Description = postCreation.Description;
        
        if(postCreation.CategoryId != 0)
        {
            post.CategoryId = postCreation.CategoryId;
        }
        
        post.Status = postCreation.isDraft ? BLOG_STATUS.DRAFT : BLOG_STATUS.PUBLISHED;

        // Step 3: Filter out TagIds that are 0 (handle them as null/ignore)
        var validTagIds = postCreation.TagIds.Where(tagId => tagId != 0).ToList();

        // Step 4: Load the tags corresponding to the valid TagIds
        var tags = await _context.Tags.Where(t => validTagIds.Contains(t.Id)).ToListAsync();

        // Step 5: Clear existing tags and add new valid ones
        post.PostTags.Clear(); // If you want to replace the old tags
        post.PostTags = tags.Select(tag => new PostTag
        {
            Post = post,
            Tag = tag
        }).ToList();

        // Step 6: Save changes
        await _context.SaveChangesAsync();

        return post;
    }
    
    public async Task<Post> AssignCoverImageAsync(int postId, string imageUrl)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post == null)
        {
            throw new NotFoundException("Post not found.");
        }
        
        post.CoverImageUrl = imageUrl;
        await _context.SaveChangesAsync();
        
        return post;
    }

    public async Task<Post> GetPostByIdAsync(int postId)
    {
        var post = await _context.Posts
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post?.PostTags != null)
        {
            Console.WriteLine("PostTags Count: " + post.PostTags.Count); // Check if it's empty or populated
        }
        else
        {
            Console.WriteLine("PostTags is null");
        }
        
        return post;
    }
    
    



    public async Task<Post> DeletePostAsync(int postId)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post == null)
        {
            throw new NotFoundException("Post not found.");
        }
        
        post.Status = BLOG_STATUS.DELETED;
        await _context.SaveChangesAsync();
        
        return post;
    }
    
    
    // get 3 suggested posts based on the current post's tags

    
    // get 3 suggested posts based on the current post's tags
    public async Task<List<PostDto>> Get3SuggestedPosts(int postId)
    {
        var post = await _context.Posts
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return null;
        }

        var tagIds = post.PostTags.Select(pt => pt.TagId).ToList();

        var suggestedPosts = await _context.PostTags
            .Include(pt => pt.Post)
            .ThenInclude(p => p.Category)
            .Include(pt => pt.Tag)
            .Where(pt => tagIds.Contains(pt.TagId) && pt.PostId != postId && pt.Post.Status == BLOG_STATUS.PUBLISHED)
            .OrderByDescending(pt => pt.Post.CreatedAt)
            .Select(pt => pt.Post)
            .Take(3)
            .Distinct()
            .ToListAsync();

        return _mapper.Map<List<PostDto>>(suggestedPosts);
    }
    
    public async Task SeedPostsAsync()
    {
        const int maxRetries = 3;
        const int delayMilliseconds = 1000;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();



                    var random = new Random();
                    var categories = await context.Categories.ToListAsync();
                    var tags = await context.Tags.ToListAsync();

                    var titles = new List<string>
                    {
                        "The Future of AI", "Sustainable Living Tips", "Exploring Mars", "Healthy Eating Habits",
                        "Cybersecurity Essentials", "Mindfulness Meditation", "Renewable Energy Trends",
                        "The Art of Photography", "Machine Learning Basics", "Travel on a Budget",
                        "Effective Time Management", "The History of Jazz", "Coding for Beginners",
                        "Climate Change Solutions", "The Psychology of Happiness", "Innovations in Healthcare",
                        "DIY Home Improvements", "Understanding Blockchain", "The Evolution of Social Media",
                        "Fitness for Busy Professionals", "The Impact of 5G", "Organic Gardening Tips",
                        "The Future of Work", "Mastering Public Speaking", "Financial Planning 101"
                    };

                    for (int i = 0; i < 25; i++)
                    {
                        var post = new Post
                        {
                            Title = titles[i],
                            Content = $"This is the content for {titles[i]}. It provides valuable insights and information about the topic.",
                            CreatedAt = DateTime.Now.AddDays(-random.Next(1, 365)),
                            CategoryId = categories[random.Next(categories.Count)].Id,
                            Status = BLOG_STATUS.PUBLISHED,
                            CoverImageUrl = $"/images/cover-{i + 1}.jpg"
                        };

                        var postTags = new List<PostTag>();
                        var tagCount = random.Next(1, 4);
                        var shuffledTags = tags.OrderBy(x => random.Next()).Take(tagCount);
                        foreach (var tag in shuffledTags)
                        {
                            postTags.Add(new PostTag { Post = post, TagId = tag.Id });
                        }
                        post.PostTags = postTags;

                        await context.Posts.AddAsync(post);
                    }

                    await context.SaveChangesAsync();
                    return; // Seeding successful, exit the method
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempt + 1} failed. Error: {ex.Message}");
                if (attempt == maxRetries - 1)
                {
                    // If this was the last attempt, rethrow the exception
                    throw;
                }
                await Task.Delay(delayMilliseconds);
            }
        }
    }
    
    // search posts by title, content, category, and tags
    public async Task<ApiResponse<PaginatedResult<PostDto>>> SearchPostsAsync(string query, int pageNumber, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("No search query provided.");
        }

        if (pageNumber < 1 || pageSize < 1)
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("Invalid page number or page size.");
        }

        var loweredQuery = query.ToLower();

        var postsQuery = _context.Posts
            .Include(p => p.Category)
            .Include(p => p.PostTags)
            .ThenInclude(pt => pt.Tag)
            .Where(p => p.Status == BLOG_STATUS.PUBLISHED &&
                        (p.Title.ToLower().Contains(loweredQuery) ||
                         p.Content.ToLower().Contains(loweredQuery) ||
                         p.Category.Name.ToLower().Contains(loweredQuery) ||
                         p.PostTags.Any(pt => pt.Tag.Name.ToLower().Contains(loweredQuery))));

        var totalCount = await postsQuery.CountAsync();

        if (totalCount == 0)
        {
            return ApiResponse<PaginatedResult<PostDto>>.FailureResponse("No posts found matching the search query.");
        }

        var posts = await postsQuery
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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

        var paginatedResult = new PaginatedResult<PostDto>(postDtos, totalCount, pageNumber, pageSize);
        return ApiResponse<PaginatedResult<PostDto>>.SuccessResponse(paginatedResult, "Posts retrieved successfully");
    }



}