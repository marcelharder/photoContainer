using api.data.dtos;

namespace api.Controllers;

public class CategoryController : BaseApiController
{
    
private readonly ICategory _cat;

    public CategoryController(ICategory cat)
    {
        _cat = cat;
    }
    
    [HttpGet("getAllCategories")]
    public async Task<IActionResult> Categories()
    {
        var result = await _cat.GetAllCategories();
        return Ok(result);
    }

    
    [HttpPost("getAllowedCategories")]
    public async Task<ActionResult> AllowedCategories([FromBody] AllowedCategoriesRequest request)
    {
        var result = await _cat.GetAllowedCategories(request.cp);
        return Ok(result);
    }

    [HttpGet("getDescription/{category}")]
    public async Task<IActionResult> GetDescription(int category)
    {
        var result = await _cat.ReadCategory(category);
        if (result == null)
        {
            return BadRequest("");
        }

        return Ok(result.Description);
    }
}
