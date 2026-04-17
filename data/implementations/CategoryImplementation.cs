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



   
    public async Task<CategoryDto[]?> GetAllCategoriesForUser(int userId)
    {
        var query = "Select * FROM Categories WHERE UserId = @userId";
        using var connection = _dap.CreateConnection();
        var documents = await connection.QueryAsync<CategoryDto>(query, new { userId });
        return documents.ToArray();
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
    public Task<CategoryDto> CreateCategory(Category up)
    {
        throw new NotImplementedException();
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
