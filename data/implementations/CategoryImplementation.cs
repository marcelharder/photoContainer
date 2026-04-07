namespace photoContainer.data.implementations;

public class CategoryImplementation : ICategory
{
    private ApplicationDbContext _context;
    private DapperContext _dap;
    private IHttpContextAccessor _ht;
    private readonly IMapper _mapper;
    private readonly IConfiguration _conf;

    public CategoryImplementation(
        IConfiguration conf,
        DapperContext dap,
        ApplicationDbContext context,
        IMapper mapper,
        IHttpContextAccessor ht
    )
    {
        _mapper = mapper;
        _context = context;
        _ht = ht;
        _dap = dap;
        _conf = conf;
    }

    public Task<CategoryDto> CreateCategory(Category up)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Category>> getCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task UpdateCategories()
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

    public async Task<PagedList<CategoryDto>?> GetAllCategories(CategoryParams cp)
    {
        var query = "Select * FROM Categories";
        using var connection = _dap.CreateConnection();
        var documents = await connection.QueryAsync<CategoryDto>(query);

        return PagedList<CategoryDto>.CreateAsync(documents, cp.PageNumber, cp.PageSize);
    }

    public async Task<PagedList<CategoryDto>?> GetAllowedCategories(CategoryParams cp)
    {
        var _result = new List<CategoryDto>();
        await Task.Run(() =>
        {
            foreach (int cat in cp.AllowedCategories)
            {
                GetSpecificCategory(cat)
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

        return PagedList<CategoryDto>.CreateAsync(_result, cp.PageNumber, cp.PageSize);
    }

    public async Task<CategoryDto?> GetSpecificCategory(int id)
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
}
