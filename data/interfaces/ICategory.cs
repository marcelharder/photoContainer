namespace photoContainer.data.interfaces;

public interface ICategory
{
    Task<CategoryDto[]?> GetAllCategories();
    Task<CategoryDto[]?> GetAllowedCategories(int[] categoryIds);
    

    Task<CategoryDto> CreateCategory(Category up);
    Task<CategoryDto?> ReadCategory(int category);
    Task UpdateCategory(CategoryDto category);
    Task DeleteCategory(int id);


}