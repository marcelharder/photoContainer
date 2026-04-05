using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using photoContainer.data.models;

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
            categories = categories.OrderBy(c => c.YearTaken).ToList();// ORDER BY Year
            
            foreach (Category im in categories) 
            {
                im.MainPhoto = counter;// set main photo
                counter = counter + im.Number_of_images - 1;
                im.Name = char.ToUpper(im.Name[0]) + im.Name.Substring(1);// MAKE FIRST CHARACTER A CAPITAL LETTER
                _ = context.Categories.Add(im);// save image to database
            }
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedImages(ApplicationDbContext context, IImage image)
    {
        var counter = 0;
        var catList = new List<Category>();
        ImageDto test;

        if (await context.Images.AnyAsync())
            return;

        catList = await image.getCategories();

        if (catList != null)
        {
            for (int x = 0; x < catList.Count; x++)
            {
                counter = 0;
                if (catList[x].Number_of_images != 0)
                {
                    counter += (int)catList[x].Number_of_images;

                    for (int y = 1; y < counter; y++)
                    {
                        string? url = catList[x].Name + "/" + y.ToString() + ".jpg";

                        test = new ImageDto
                        {
                            //Id = x.ToString(),
                            ImageUrl = url,
                            YearTaken = 1995,
                            Location = "",
                            Familie = "",
                            Category = catList[x].Id,
                            Series = "",
                            Spare1 = "",
                            Spare2 = "",
                            Spare3 = "",
                        };
                        await image.addImage(test);
                    }
                }

                //addImage(catList[x],counter,url,_dapper);
            }
        }
    }

   
}
