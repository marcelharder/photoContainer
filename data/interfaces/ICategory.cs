namespace photoContainer.data.interfaces;

public interface ICategory
{
    Task<Category> CreateCategory(Category up);
    Task<List<Category>> GetAllCategories(CategoryParams cp);
    Task<List<Category>> GetAllowedCategories(CategoryParams cp);
    Task<Category> GetSpecificCategory(int category);
}