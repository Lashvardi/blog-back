using Blog.Dtos;
using Blog.Models.Blog.Enums;

namespace Blog.Services.Abstraction;

public interface ICategoryService
{
    Task<Category> CreateCategoryAsync(CategoryCreation categoryCreation);
    
    // create multiple categories
    Task<IEnumerable<Category>> CreateCategoriesAsync(IEnumerable<CategoryCreation> categoryCreations);
    
    // edit category
    Task<Category> EditCategoryAsync(int categoryId, CategoryCreation categoryCreation);
    
    // delete category
    Task<bool> DeleteCategoryAsync(int categoryId);
    
    // get all categories
    Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync();
}