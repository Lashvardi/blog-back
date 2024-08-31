using System.Text.Json.Serialization;

namespace Blog.Models.Blog;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
        
    [JsonIgnore]
    public ICollection<PostTag> PostTags { get; set; }
}