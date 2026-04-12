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
   
   
   
   // CRUD operations for categories
   
   
    [HttpGet("getSpecificCategory/{category}")]
    public async Task<IActionResult> GetSpecificCategory(int category)
    {
        var result = await _cat.ReadCategory(category);
        if (result == null)
        {
            return BadRequest("");
        }

        return Ok(result);
    }
    [HttpPut("updateCategory")]
    public async Task<ActionResult> UpdateCategory([FromBody] CategoryDto category)
    {
        await _cat.UpdateCategory(category);
        return Ok("Category was updated");
    }

    [HttpPost("addCategory")]
    public async Task<ActionResult> AddCategory([FromBody] CategoryDto category)
    {
        var categoryModel = new Category
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            MainPhoto = 0,
            Number_of_images = 0,
            YearTaken = 2026
        };

        var result = await _cat.CreateCategory(categoryModel);
        if (result != null)
        {
            return Ok("Category was added");
        }
        return BadRequest("Category was not added");
    }

    [HttpDelete("deleteCategory/{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
         var result = await _cat.DeleteCategory(id);
        if (result == 1)
        {
            return Ok("Category was deleted");
        }
        return BadRequest("Category was not deleted");
    }
   
}
