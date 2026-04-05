namespace photoContainer.data.implementations;

public class Dappercategory : IDapperCategoryService
{
    private readonly DapperContext _context;

    public Dappercategory(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> SeedImages()
    {
        var query = "Select * FROM Categories";
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<CategoryDto>(query);

        var catList = documents.ToList();
        var imageList = new List<ImageDto>();
        var counter = 0;
        var offset = 1;
        var imagecounter = 0;

        for (int x = 0; x < catList.Count; x++)
        {
            counter += (int)catList[x].Number_of_images;
            for (int y = offset; y < counter; y++) // get the list of images that belongs to this category
            {
                imagecounter++;
                imageList.Add(
                    new ImageDto
                    {
                        Id = y.ToString(),
                        Location = catList[x].Description,
                        ImageUrl = catList[x].Name + "/" + imagecounter + ".jpg",
                        YearTaken = 1955,
                        Category = (int)catList[x].Id,
                        Familie = "",
                        Quality = "",
                        Series = "",
                        Spare1 = "",
                        Spare2 = "",
                        Spare3 = "",
                    }
                );
            }
            foreach (ImageDto im in imageList)
            {
                await AddImage(im);
            }
            offset = counter + 1;
            imagecounter = 0;
            imageList.Clear();
        }
        return 1;
    }

    public Task<Category> CreateCategory(Category up)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedList<CategoryDto>?> GetAllCategories(CategoryParams cp)
    {
        var query = "Select * FROM Categories";
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<CategoryDto>(query);

        return PagedList<CategoryDto>.CreateAsync(documents, cp.PageNumber, cp.PageSize);
    }

    public async Task<PagedList<CategoryDto>?> GetAllowedCategories(CategoryParams cp)
    {

        var _result = new List<CategoryDto>();
        
        var query = "Select * FROM Categories";
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<CategoryDto>(query);
        var allCategories = documents.ToList();
        
        foreach (CategoryDto cat in allCategories)
        {
            if (cat.Description != null)
            {
                var id = cat.Id;
                if (cp.AllowedCategories.Contains((int)id))
                {
                    var help = new CategoryDto
                    {
                        Id = id,
                        Description = cat.Description,
                        MainPhoto = cat.MainPhoto,
                        Number_of_images = 0,
                        YearTaken = cat.YearTaken
                    };
                    _result.Add(help);
                }
                // sort op year
                 _result = _result.OrderBy(o => o.YearTaken).ToList();
          }
        }
        return PagedList<CategoryDto>.CreateAsync(_result, cp.PageNumber, cp.PageSize);
    }

    public async Task<PagedList<ImageDto>?> GetImagesByCategory(ImageParams ip)
    {
        var categoryId = ip.Category;
        var query = "Select * FROM Images Where Category = @categoryId";
        /// select correct category
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<ImageDto>(query, new { categoryId });
        var selectedImages = documents.ToList();
        var _result = new List<ImageDto>();
        foreach (ImageDto img in selectedImages)
        {
            var help = new ImageDto
            {
                Id = img.Id,
                ImageUrl = img.ImageUrl,
                YearTaken = img.YearTaken,
                Location = img.Location,
                Familie = img.Familie,
                Category = categoryId,
                Quality = img.Quality,
                Series = img.Series,
                Spare1 = img.Spare1,
                Spare2 = img.Spare2,
                Spare3 = img.Spare3
            };
            _result.Add(help);
        }
        return PagedList<ImageDto>.CreateAsync(_result, ip.PageNumber, ip.PageSize);
    }

    public async Task<CategoryDto?> GetSpecificCategory(int id)
    {
        var query = "Select * FROM Categories WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            var document = await connection.QueryFirstOrDefaultAsync<CategoryDto>(query, new { id });

            return document;
        }
    }

    public Task UpdateCategory(Category up)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddImage(ImageDto test)
    {
        var query =
            "INSERT INTO Images (Id,ImageUrl,YearTaken,Location,Familie,Category,Series,Quality,Spare1,Spare2,Spare3)"
            + "VALUES(@Id,@ImageUrl,@YearTaken,@Location,@Familie,@Category,@Series,@Quality,@Spare1,@Spare2,@Spare3)";

        var parameters = new DynamicParameters();
        parameters.Add("Id", test.Id, DbType.Int32);
        parameters.Add("ImageUrl", test.ImageUrl, DbType.String);
        parameters.Add("YearTaken", 1955, DbType.Int32);
        parameters.Add("Location", test.Location, DbType.String);
        parameters.Add("Familie", "n/a", DbType.String);
        parameters.Add("Category", test.Category, DbType.Int32);
        parameters.Add("Series", "n/a", DbType.String);
        parameters.Add("Quality", "n/a", DbType.String);
        parameters.Add("Spare1", "n/a", DbType.String);
        parameters.Add("Spare2", "n/a", DbType.String);
        parameters.Add("Spare3", "n/a", DbType.String);

        using var connection = _context.CreateConnection();
        return await connection.ExecuteAsync(query, parameters);
    }

    public async Task<PagedList<ImageDto>?> GetFilesForUser(ImageParams ip)
    {
        var categoryId = ip.Category;
        var query = "Select * FROM Images";
        /// select correct category
        using var connection = _context.CreateConnection();
        var documents = await connection.QueryAsync<ImageDto>(query);
        var selectedImages = documents.ToList();
        var _result = new List<ImageDto>();
        foreach (ImageDto img in selectedImages)
        {
            var help = new ImageDto
            {
                Id = img.Id,
                ImageUrl = img.ImageUrl,
                YearTaken = img.YearTaken,
                Location = img.Location,
                Familie = img.Familie,
                Category = categoryId,
                Quality = img.Quality,
                Series = img.Series,
                Spare1 = img.Spare1,
                Spare2 = img.Spare2,
                Spare3 = img.Spare3,
                Spare4 = transformToStringArray(img.Spare1)
            };

            if (help.Spare4 != null)
            {
                if (help.Spare4.Contains(ip.Id.ToString())) { _result.Add(help); }
            }
        }
        return PagedList<ImageDto>.CreateAsync(_result, ip.PageNumber, ip.PageSize);
    }

    public string[]? transformToStringArray(string? test)
    {
        if (test != null)
        {
            var h = test.Split(",");
            return h;
        }
        else return null;

    }



}
