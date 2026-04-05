namespace api.Controllers;

public class CategoryController : BaseApiController
{
    private readonly IDapperCategoryService _cat;


    public CategoryController(IDapperCategoryService cat)
    {
        _cat = cat;
    }


    [HttpPost("getAllCategories")]
    public async Task<IActionResult> Categories([FromBody] CategoryParams cp)
    {
        var result = await _cat.GetAllCategories(cp);
        return Ok(result);
    }


    [HttpPost("getAllowedCategories")]
    public async Task<ActionResult> AllowedCategories([FromBody] CategoryParams cp)
    {
        var result = await _cat.GetAllowedCategories(cp);
        return Ok(result);
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
