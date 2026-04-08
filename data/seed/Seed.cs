namespace photoContainer.data.seed;

public class Seed
{
    public static async Task SeedCategories(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var counter = 1;
        var catData = await System.IO.File.ReadAllTextAsync("data/seed/CategoryData.json");
        var categories = JsonSerializer.Deserialize<List<Category>>(catData);

        if (categories != null)
        {
            categories = categories.OrderBy(c => c.YearTaken).ToList(); // ORDER BY Year

            foreach (Category im in categories)
            {
                im.MainPhoto = counter; // set main photo
                counter = counter + im.Number_of_images - 1;
                im.Name = char.ToUpper(im.Name[0]) + im.Name.Substring(1); // MAKE FIRST CHARACTER A CAPITAL LETTER
                _ = context.Categories.Add(im); // save image to database
            }
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedImages(ApplicationDbContext context, IImage image, ICategory categoryService)
    {
        if (await context.Images.AnyAsync())
            return;

        var categories = await categoryService.getCategories();

        if (categories == null || categories.Count == 0)
            return;

        var images = new List<models.Image>();

        //string nasRoot = @"\\192.168.2.22\photoproject\fotos"; // replace "photos" with your Synology share name

        foreach (var category in categories)
        {

            var segments = category.Name.Split(new[] { '/','\\'}, StringSplitOptions.RemoveEmptyEntries);

            var directoryPath = Path.Combine(new[] {"/nfs/fotoproject/fotos"}.Concat(segments).ToArray());

            if (!Directory.Exists(directoryPath))
                continue;

            int imageCount = 0;

            try
            {
                imageCount = Directory.EnumerateFiles(directoryPath, "*.jpg", SearchOption.TopDirectoryOnly).Count();
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }
            catch (IOException)
            {
                continue;
            }

            for (int i = 1; i <= imageCount; i++)
            {
                images.Add(new models.Image
                {
                    ImageUrl = $"{category.Name}/{i}.jpg",
                    YearTaken = 1995,
                    Location = string.Empty,
                    Familie = string.Empty,
                    Category = category.Id,
                    Series = string.Empty,
                    Spare1 = string.Empty,
                    Spare2 = string.Empty,
                    Spare3 = string.Empty
                });
            }
        }

        if (images.Count > 0)
        {
            await context.Images.AddRangeAsync(images);
            await context.SaveChangesAsync();
        }
    }
}

