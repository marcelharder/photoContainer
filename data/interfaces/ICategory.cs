namespace photoContainer.data.interfaces;

public interface ICategory
{
    Task<CategoryDto[]?> GetAllCategories();
    Task<CategoryDto[]?> GetAllowedCategories(CategoryParams cp);
    

    Task<CategoryDto> CreateCategory(Category up);
    Task<CategoryDto?> ReadCategory(int category);
    Task UpdateCategory(CategoryDto category);
    Task DeleteCategory(int id);


}