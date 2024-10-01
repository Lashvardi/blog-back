using Blog.Models.Blog.Enums;

namespace Blog.Dtos;

public class GetPostDtoWithoutContent
{
    public int Id { get; set; }
    public string Title { get; set; }
    public BLOG_STATUS Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    
    public string CoverImageUrl { get; set; }
    
    public string FormattedElapsedTimeSinceCreation { get; set; }
    public int? CategoryId { get; set; }
    public CategoryDto Category { get; set; }
    public List<PostTagDto> PostTags { get; set; }
}

