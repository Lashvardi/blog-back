using Blog.Dtos;
using Blog.Models.Blog;

namespace Blog.Services.Abstraction;

public interface ITagService
{
    Task<Tag> CreateTagAsync(TagCreation tagCreation);
    Task<IEnumerable<TagDto>> CreateTagsAsync(IEnumerable<TagCreation> tagCreations);
    Task<TagDto> EditTagAsync(int tagId, TagCreation tagCreation);
    Task<bool> DeleteTagAsync(int tagId);
    
    Task<IEnumerable<TagDto>> GetTagsAsync();
    
}