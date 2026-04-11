using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace photoContainer.data.implementations;

public class ImageImplementation : IImage
{
    private ApplicationDbContext _context;
    private DapperContext _dap;
    private ICategory _category;
    private IHttpContextAccessor _ht;


    public ImageImplementation(
        IConfiguration conf,
        DapperContext dap,
        ApplicationDbContext context,
        ICategory category,
        IHttpContextAccessor ht
        )
    {
        _context = context;
        _ht = ht;
        _dap = dap;
        _category = category;
    }

    public async Task<ImageDto[]> getAllImages()
    {
        var query = "Select * FROM Images";
        using var connection = _dap.CreateConnection();
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
                Category = img.Category,
                Quality = img.Quality,
                Series = img.Series,
                Spare1 = img.Spare1,
                Spare2 = img.Spare2,
                Spare3 = img.Spare3
            };
            _result.Add(help);
        }
        return _result.ToArray();
    }
    public async Task<ImageDto[]> GetImagesByCategory(ImageParams ip)
    {
        var categoryId = ip.Category;
        var query = "Select * FROM Images Where Category = @categoryId";
        /// select correct category
        using var connection = _dap.CreateConnection();
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
                Category = img.Category,
                Quality = img.Quality,
                Series = img.Series,
                Spare1 = img.Spare1,
                Spare2 = img.Spare2,
                Spare3 = img.Spare3
            };
            _result.Add(help);
        }
        return _result.ToArray();
    }

    public async Task<ImageDto> ReadImage(int Id)
    {
        var query = "Select * FROM Images Where Id = @Id";
        using var connection = _dap.CreateConnection();
        var documents = await connection.QueryAsync<ImageDto>(query, new { Id });
        var selectedImage = documents.FirstOrDefault();
        if (selectedImage != null)
        {
            return new ImageDto
            {
                Id = selectedImage.Id,
                ImageUrl = selectedImage.ImageUrl,
                YearTaken = selectedImage.YearTaken,
                Location = selectedImage.Location,
                Familie = selectedImage.Familie,
                Category = selectedImage.Category,
                Quality = selectedImage.Quality,
                Series = selectedImage.Series,
                Spare1 = selectedImage.Spare1,
                Spare2 = selectedImage.Spare2,
                Spare3 = selectedImage.Spare3
            };
        }
        else
        {
            return null;
        }
        /*  var selectedImage = await _context.Images.FirstOrDefaultAsync(x => x.Id == Id);
         return _mapper.Map<ImageDto>(selectedImage); */
    }

    public async Task<ImageDto[]> findImagesByUser(CategoryParams cp)
    {
        // find all images that have the current userid in spare1
        var imageArray = await getAllImages();
        imageArray = imageArray.Where(img =>
          {
              var tags = TransformToStringArray(img.Spare1);
              return tags != null && tags.Contains(cp.userId.ToString());
          }).ToArray();
        return imageArray;
    }

    public async Task<int> deleteImage(int id)
    {
        // remove this image with dapper
        var query = "DELETE FROM Images WHERE Id = @id";
        using var connection = _dap.CreateConnection();
        return await connection.ExecuteAsync(query, new { id });
    }

    public async Task<int> updateImage(ImageDto imagedto)
    {
        var query = @"
        UPDATE Images
        SET ImageUrl = @ImageUrl,
            YearTaken = @YearTaken,
            Location = @Location,
            Familie = @Familie,
            Category = @Category,
            Series = @Series,
            Quality = @Quality,
            Spare1 = @Spare1,
            Spare2 = @Spare2,
            Spare3 = @Spare3
        WHERE Id = @Id";

        using var connection = _dap.CreateConnection();
        return await connection.ExecuteAsync(query, imagedto);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<int> createImage(ImageDto test)
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

        using var connection = _dap.CreateConnection();
        return await connection.ExecuteAsync(query, parameters);
    }
    public async Task<ActionResult<CarouselDto>> getCarouselData(int id)
    {
        var response = new CarouselDto();
        var selectedImage = await _context.Images.FirstOrDefaultAsync(x => x.Id == id);
        if (selectedImage != null)
        {
            var images = await _context
                .Images.Where(x => x.Category == selectedImage.Category)
                .ToArrayAsync();
            var test = images.ToList();
            var numberOfImages = test.Count();

            if (numberOfImages == 1)
            {
                response.numberOfImages = 1;
                response.ShowL = false;
                response.ShowR = false;
                response.nextImageIdL = null;
                response.nextImageIdR = null;
            }
            else
            {
                var lastImage = test.LastOrDefault();
                var firstImage = test.FirstOrDefault();
                int imagelocation = test.FindIndex(x => x == selectedImage);

                if (imagelocation == 0) // dit is het eerste item
                {
                    response.numberOfImages = test.Count();
                    response.ShowL = false;
                    response.ShowR = true;
                    response.nextImageIdR = test[imagelocation + 1].Id;
                }
                else
                {
                    if (imagelocation == (numberOfImages - 1)) // dit is het laatste item
                    {
                        response.numberOfImages = test.Count();
                        response.ShowR = false;
                        response.ShowL = true;
                        response.nextImageIdL = test[imagelocation - 1].Id;
                    }
                    else
                    {
                        response.numberOfImages = test.Count();
                        response.ShowL = true;
                        response.ShowR = true;
                        response.nextImageIdL = test[imagelocation - 1].Id;
                        response.nextImageIdR = test[imagelocation + 1].Id;
                    }
                }

                response.category = selectedImage.Category;
                return response;
            }
        }
        return null;
    }

    public async Task<PagedList<ImageDto>?> GetFilesForUser(CategoryParams ip)
    {
        var allowedCategories = await _category.GetAllowedCategories(ip);

        if (allowedCategories == null || !allowedCategories.Any())
            return PagedList<ImageDto>.CreateAsync(
                new List<ImageDto>(),
                ip.PageNumber,
                ip.PageSize
            );

        var categoryIds = allowedCategories.Select(c => c.Id).ToArray();

        using var connection = _dap.CreateConnection();

        var query =
            @"
        SELECT *
        FROM Images
        WHERE Category IN @categoryIds";

        var documents = await connection.QueryAsync<ImageDto>(query, new { categoryIds });

        var filtered = documents
            .Where(img =>
            {
                var tags = TransformToStringArray(img.Spare1);
                return tags != null && tags.Contains(ip.userId.ToString());
            })
            .ToList();

        return PagedList<ImageDto>.CreateAsync(filtered, ip.PageNumber, ip.PageSize);
    }

    public string[]? TransformToStringArray(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();
    }
    public Task<ImageDto?> getMainImageOfCategory(int categoryId)
    {
        //get the image with the lowest id of this category
        var query = "Select * FROM Images Where Category = @categoryId ORDER BY Id ASC LIMIT 1";
        using var connection = _dap.CreateConnection();
        var documents = connection.QueryAsync<ImageDto>(query, new { categoryId }).Result;
        var selectedImage = documents.FirstOrDefault();
        if (selectedImage != null)
        {
            return Task.FromResult<ImageDto?>(new ImageDto
            {
                Id = selectedImage.Id,
                ImageUrl = selectedImage.ImageUrl,
                YearTaken = selectedImage.YearTaken,
                Location = selectedImage.Location,
                Familie = selectedImage.Familie,
                Category = selectedImage.Category,
                Quality = selectedImage.Quality,
                Series = selectedImage.Series,
                Spare1 = selectedImage.Spare1,
                Spare2 = selectedImage.Spare2,
                Spare3 = selectedImage.Spare3
            });
        }

        else { return Task.FromResult<ImageDto?>(null); }
    }



}
