using Blog.Dtos;
using Blog.Models.Blog.Enums;
using Blog.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Blog;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpPost("create-category")]
    public async Task<ApiResponse<Category>> CreateCategoryAsync([FromBody] CategoryCreation categoryCreation)
    {
        var category = await _categoryService.CreateCategoryAsync(categoryCreation);
        
        return ApiResponse<Category>.SuccessResponse(category, "Category created successfully");
    }
    
    [HttpPost("create-categories")]
    public async Task<ApiResponse<IEnumerable<Category>>> CreateCategoriesAsync([FromBody] IEnumerable<CategoryCreation> categoryCreations)
    {
        var categories = await _categoryService.CreateCategoriesAsync(categoryCreations);
        
        return ApiResponse<IEnumerable<Category>>.SuccessResponse(categories, "Categories created successfully");
    }
    
    [HttpPut("edit-category/{categoryId}")]
    public async Task<ApiResponse<Category>> EditCategoryAsync(int categoryId, [FromBody] CategoryCreation categoryCreation)
    {
        var category = await _categoryService.EditCategoryAsync(categoryId, categoryCreation);
        
        if (category == null)
        {
            return ApiResponse<Category>.FailureResponse("Category not found");
        }
        
        return ApiResponse<Category>.SuccessResponse(category, "Category edited successfully");
    }
    
    [HttpDelete("delete-category/{categoryId}")]
    public async Task<ApiResponse<bool>> DeleteCategoryAsync(int categoryId)
    {
        var isDeleted = await _categoryService.DeleteCategoryAsync(categoryId);
        
        if (!isDeleted)
        {
            return ApiResponse<bool>.FailureResponse("Category not found");
        }
        
        return ApiResponse<bool>.SuccessResponse(isDeleted, "Category deleted successfully");
    }
    
    [HttpGet("get-categories")]
    public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetCategoriesAsync()
    {
        var categories = await _categoryService.GetCategoriesAsync();

        return categories;
    }
    
}