using AutoMapper;
using Blog.Data;
using Blog.Dtos;
using Blog.Models.Blog.Enums;
using Blog.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Implementation;

public class CategoryService : ICategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    
    public CategoryService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Category> CreateCategoryAsync(CategoryCreation categoryCreation)
    {
        var category = new Category
        {
            Name = categoryCreation.Name
        };
        
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        
        return category;
    }
    
    public async Task<IEnumerable<Category>> CreateCategoriesAsync(IEnumerable<CategoryCreation> categoryCreations)
    {
        var categories = categoryCreations.Select(categoryCreation => new Category
        {
            Name = categoryCreation.Name
        }).ToList();
        
        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
        
        return categories;
    }
    
    public async Task<Category> EditCategoryAsync(int categoryId, CategoryCreation categoryCreation)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        
        if (category == null)
        {
            return null;
        }
        
        category.Name = categoryCreation.Name;
        
        await _context.SaveChangesAsync();
        
        return category;
    }
    
    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        
        if (category == null)
        {
            return false;
        }
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        
        if (categories == null || !categories.Any())
        {
            return ApiResponse<IEnumerable<CategoryDto>>.FailureResponse("No categories found");

        }
        
        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        
        return ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categoryDtos, "Categories found successfully");
        
    }

}