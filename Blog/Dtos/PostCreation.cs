namespace Blog.Dtos;

public class PostCreation
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public List<int> TagIds { get; set; }
    
    public bool isDraft { get; set; }

}