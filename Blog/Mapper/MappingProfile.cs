using AutoMapper;
using Blog.Dtos;
using Blog.Models.Blog;
using Blog.Models.Blog.Enums;

namespace Blog.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDto>();
        CreateMap<PostCreation, Post>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Tag, TagDto>();
        CreateMap<PostTag, PostTagDto>();
        
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.FormattedElapsedTimeSinceCreation, opt => opt.Ignore());

        CreateMap<PostCreation, Post>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.isDraft ? BLOG_STATUS.DRAFT : BLOG_STATUS.PUBLISHED))
            .ForMember(dest => dest.PostTags, opt => opt.Ignore());

        CreateMap<Category, CategoryDto>();
        CreateMap<Tag, TagDto>();

        CreateMap<PostTag, PostTagDto>()
            .ForMember(dest => dest.Tag, opt => opt.MapFrom(src => src.Tag));

        // Reverse mappings if needed
        CreateMap<PostDto, Post>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (BLOG_STATUS)src.Status));

        CreateMap<CategoryDto, Category>();
        CreateMap<TagDto, Tag>();

        
    }
    
}