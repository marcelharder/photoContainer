namespace photoContainer.data.interfaces;

public interface ICategory
{
    Task<CategoryDto> CreateCategory(Category up);
    Task<PagedList<CategoryDto>> GetAllCategories(CategoryParams cp);
    Task<PagedList<CategoryDto>> GetAllowedCategories(CategoryParams cp);
    Task<CategoryDto?> GetSpecificCategory(int category);

    Task<List<Category>> getCategories();

     Task UpdateCategories();
}