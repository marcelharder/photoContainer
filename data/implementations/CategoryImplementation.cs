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



    public async Task UpdateCategory(CategoryDto category)
    {
        // fill a list with the id of the first image of each category
        List<int> listOfIds = new List<int>();
        var counter = 0;

        var allCategories = await _context.Categories.ToListAsync();
        foreach (Category cat in allCategories)
        {
            var images = _context.Images.Where(global => global.Category == cat.Id).ToList();
            listOfIds.Add(images[0].Id);
        }

        // with this list update the Categories
        foreach (Category cat in allCategories)
        {
            cat.MainPhoto = listOfIds[counter];
            _context.Categories.Update(cat);
            await _context.SaveChangesAsync();
            counter++;
        }
        //  var allImages = await context.Images.ToListAsync();
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
    public Task UpdateCategory(Category up)
    {
        throw new NotImplementedException();
    }
    public Task DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }
}
