using Blog.Models.Blog.Enums;

namespace Blog.Models.Blog;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } // Markdown or HTML content based on wysiwyg editor
    
    public BLOG_STATUS Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
        
    // Foreign key for Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }
        
    public ICollection<PostTag> PostTags { get; set; }
}