namespace api.Controllers;

public class CategoryController : BaseApiController
{
    
private readonly ICategory _cat;

    public CategoryController(ICategory cat)
    {
        _cat = cat;
    }

    [HttpGet("getCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _cat.getCategories();
        return Ok(result);
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
