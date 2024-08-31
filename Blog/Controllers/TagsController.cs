using Blog.Dtos;
using Blog.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Models.Blog;

namespace Blog
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TagsController : Controller
    {
        private readonly ITagService tagService;

        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [HttpPost("create-tag")]
        public async Task<ActionResult<ApiResponse<Tag>>> CreateTagAsync([FromBody] TagCreation tagCreation)
        {
            var tag = await tagService.CreateTagAsync(tagCreation);
            return Ok(ApiResponse<Tag>.SuccessResponse(tag, "Tag created successfully"));
        }

        [HttpPost("create-tags")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TagDto>>>> CreateTagsAsync([FromBody] IEnumerable<TagCreation> tagCreations)
        {
            var tags = await tagService.CreateTagsAsync(tagCreations);
            return Ok(ApiResponse<IEnumerable<TagDto>>.SuccessResponse(tags, "Tags created successfully"));
        }

        [HttpPut("edit-tag/{tagId}")]
        public async Task<ActionResult<ApiResponse<TagDto>>> EditTagAsync(int tagId, [FromBody] TagCreation tagCreation)
        {
            var tag = await tagService.EditTagAsync(tagId, tagCreation);
            if (tag == null)
            {
                return NotFound(ApiResponse<TagDto>.FailureResponse($"Tag with ID {tagId} not found"));
            }
            return Ok(ApiResponse<TagDto>.SuccessResponse(tag, "Tag updated successfully"));
        }

        [HttpDelete("delete-tag/{tagId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTagAsync(int tagId)
        {
            var isDeleted = await tagService.DeleteTagAsync(tagId);
            if (!isDeleted)
            {
                return NotFound(ApiResponse<bool>.FailureResponse($"Tag with ID {tagId} not found"));
            }
            return Ok(ApiResponse<bool>.SuccessResponse(true, "Tag deleted successfully"));
        }
        
        [HttpGet("get-tags")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TagDto>>>> GetTagsAsync()
        {
            var tags = await tagService.GetTagsAsync();
            return Ok(ApiResponse<IEnumerable<TagDto>>.SuccessResponse(tags, "Tags retrieved successfully"));
        }
    }
}