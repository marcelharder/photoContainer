namespace api.Controllers;

public class CategoryController : BaseApiController
{
    private readonly IDapperCategoryService _cat;
    

    public CategoryController(IDapperCategoryService cat)
    {
        _cat = cat;    
    }

    [Authorize]
    [HttpGet("getAllCategories")]
    public async Task<IActionResult> Categories([FromQuery] CategoryParams cp)
    {
        var result = await _cat.GetAllCategories(cp);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("getAllowedCategories")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> AllowedCategories([FromQuery] CategoryParams cp)
    {
        if (cp.AllowedCategories.Count != 0)
            {
                var result = await _cat.GetAllowedCategories(
                    cp.AllowedCategories,
                    cp
                );
                var test = new PaginationHeader(
                    result!.CurrentPage,
                    result!.PageSize,
                    result!.TotalCount,
                    result!.TotalPages
                );
                Response.AddPaginationHeader(test);
                return Ok(result);
            }
        
        return BadRequest("");
    }

    [HttpGet("getDescription/{category}")]
    public async Task<IActionResult> GetDescription(int category)
    {
        var result = await _cat.GetSpecificCategory(category);
        if (result == null)
        {
            return BadRequest("");
        }

        return Ok(result.Description);
    }
}
