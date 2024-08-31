using AutoMapper;
using Blog.Data;
using Blog.Dtos;
using Blog.Models.Blog;
using Blog.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Implementation;

public class TagService : ITagService
{
    private readonly DataContext _context;
    private IMapper _mapper;
    
    public TagService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Tag> CreateTagAsync(TagCreation tagCreation)
    {
        var tag = new Tag
        {
            Name = tagCreation.Name
        };
        
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
        
        return tag;
    }
    
    public async Task<IEnumerable<TagDto>> CreateTagsAsync(IEnumerable<TagCreation> tagCreations)
    {
        var tags = tagCreations.Select(tagCreation => new Tag
        {
            Name = tagCreation.Name
        }).ToList();
        
        await _context.Tags.AddRangeAsync(tags);
        await _context.SaveChangesAsync();
        
        
        
        return _mapper.Map<IEnumerable<TagDto>>(tags);
    }
    
    public async Task<TagDto> EditTagAsync(int tagId, TagCreation tagCreation)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        
        if (tag == null)
        {
            return null;
        }
        
        tag.Name = tagCreation.Name;
        
        await _context.SaveChangesAsync();
        
        return _mapper.Map<TagDto>(tag);
    }
    
    public async Task<bool> DeleteTagAsync(int tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        
        if (tag == null)
        {
            return false;
        }
        
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<IEnumerable<TagDto>> GetTagsAsync()
    {
        var tags = await _context.Tags.ToListAsync();
        
        return _mapper.Map<IEnumerable<TagDto>>(tags);
        
    }
    

}