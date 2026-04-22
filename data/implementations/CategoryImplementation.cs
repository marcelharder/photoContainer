namespace photoContainer.data.implementations;

public class CategoryImplementation : ICategory
{
    private ApplicationDbContext _context;
    private DapperContext _dap;

    public CategoryImplementation(
        DapperContext dap,
        ApplicationDbContext context
)
    {
        _context = context;
        _dap = dap;
        }
    public async Task<CategoryDto[]?> GetAllCategories()
    {
        var query = "Select * FROM Categories";
        using var connection = _dap.CreateConnection();
        var documents = await connection.QueryAsync<CategoryDto>(query);
        return documents.ToArray();
    }
    public async Task<CategoryDto[]?> GetAllowedCategories(int[] categoryIds)
    {
        var _result = new List<CategoryDto>();
        await Task.Run(() =>
        {
            foreach (int cat in categoryIds)
            {
                ReadCategory(cat)
                    .ContinueWith(task =>
                    {
                        var category = task.Result;
                        if (category != null)
                        {
                            _result.Add(category);
                        }
                    })
                    .Wait();
            }
        });

        return _result.ToArray();
    }
    public async Task<CategoryDto> CreateCategory(Category up)
    {
        var query = "INSERT INTO Categories (Name, Description, MainPhoto) VALUES (@Name, @Description, @MainPhoto); SELECT LAST_INSERT_ID();";
        using var connection = _dap.CreateConnection();
        var id = await connection.ExecuteScalarAsync<int>(query, new { up.Name, up.Description, up.MainPhoto });

        return new CategoryDto
        {
            Id = id,
            Name = up.Name,
            Description = up.Description,
            MainPhoto = up.MainPhoto,
            Number_of_images = 0,
            YearTaken = 2020
        };
    } 
    public async Task<CategoryDto?> ReadCategory(int id)
    {
        var query = "Select * FROM Categories WHERE Id = @id";
        using (var connection = _dap.CreateConnection())
        {
            var document = await connection.QueryFirstOrDefaultAsync<CategoryDto>(
                query,
                new { id }
            );
            return document;
        }
    }
    public Task<int> UpdateCategory(CategoryDto up)
    {
        var query = "UPDATE Categories SET Name = @Name, Description = @Description, MainPhoto = @MainPhoto WHERE Id = @Id";
        using var connection = _dap.CreateConnection();
        connection.Execute(query, new { up.Name, up.Description, up.MainPhoto, up.Id });
        return Task.FromResult(1); 
    }
    public Task<int> DeleteCategory(int id)
    {
        var query = "DELETE FROM Categories WHERE Id = @id";
        using var connection = _dap.CreateConnection();
        connection.Execute(query, new { id });
        return Task.FromResult(1);   
    }

    
}
