using System.Text.Json.Serialization;

namespace Blog.Models.Blog.Enums;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
        
    
    [JsonIgnore]
    public ICollection<Post> Posts { get; set; }
}