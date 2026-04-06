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

    public Task<Category> CreateCategory(Category up)
    {
        throw new NotImplementedException();
    }

    public Task<List<Category>> GetAllCategories(CategoryParams cp)
    {
        throw new NotImplementedException();
    }

    public Task<List<Category>> GetAllowedCategories(CategoryParams cp)
    {
        throw new NotImplementedException();
    }

    public Task<Category> GetSpecificCategory(int category)
    {
        throw new NotImplementedException();
    }
}