namespace photoContainer.data.interfaces;
public interface IDapperCategoryService
    {
        public Task<PagedList<CategoryDto>?> GetAllCategories(CategoryParams cp);
        public Task<PagedList<ImageDto>?> GetImagesByCategory(ImageParams ip);

        public Task<PagedList<ImageDto>?> GetFilesForUser(ImageParams ip);
        public Task<PagedList<CategoryDto>?> GetAllowedCategories(CategoryParams cp);
        public Task<CategoryDto?> GetSpecificCategory(int id);
        public Task UpdateCategory(Category up);
        public Task DeleteCategory(int id);
        public Task<Category> CreateCategory(Category up);
        public Task<int> AddImage(ImageDto im);
        public Task<int> SeedImages();
    }