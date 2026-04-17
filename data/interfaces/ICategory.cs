namespace photoContainer.data.interfaces;

public interface ICategory
{
    

    Task<CategoryDto[]?> GetAllCategories();
    Task<CategoryDto[]?> GetAllowedCategories(int[] categoryIds);
    

    Task<CategoryDto> CreateCategory(Category up);
    Task<CategoryDto?> ReadCategory(int category);
    Task<int> UpdateCategory(CategoryDto category);
    Task<int> DeleteCategory(int id);


}