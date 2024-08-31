using System.Text.Json.Serialization;

namespace Blog.Models.Blog;

public class PostTag
{
    public int PostId { get; set; }
    
    [JsonIgnore]
    public Post Post { get; set; }
        
    public int TagId { get; set; }
    
    [JsonIgnore]
    public Tag Tag { get; set; }
}