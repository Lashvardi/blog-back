using System.Text.Json.Serialization;
using Blog.Models.Blog.Enums;

namespace Blog.Models.Blog;

public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    
    public string? Description { get; set; }
    
    public BLOG_STATUS Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? CoverImageUrl { get; set; }
    

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public ICollection<PostTag>? PostTags { get; set; }
}