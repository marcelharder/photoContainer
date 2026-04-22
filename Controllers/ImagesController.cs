using IronSoftware.Drawing;
using SixLabors.ImageSharp.Processing;

namespace api.Controllers;

public class ImagesController : BaseApiController
{
    private readonly IImage _image;
    private readonly IConfiguration _conf;

    public ImagesController(IImage image,IConfiguration conf)
    {
        _image = image;
        _conf = conf;
    }

  
    [HttpGet("getImageFile/{id}")]
    public async Task<IActionResult> getImageFile(int id)
    {
        var locationPrefix = _conf.GetValue<string>("NfsLocation");
        AnyBitmap anyBitmap;

        var selectedImage = await _image.ReadImage(id);
        var img = System.IO.File.OpenRead(locationPrefix + selectedImage.ImageUrl);

        using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(img))
        {
            int width = image.Width / 4;
            int height = image.Height / 4;
            image.Mutate(x => x.Resize(width, height));

            anyBitmap = image;
            var help = anyBitmap.ExportBytesAsJpg();

            return File(help, "image/jpg");
        }
    }

    [HttpGet("getFullImageFile/{id}")]
    public async Task<IActionResult> getFullImageFile(int id)
    {
        var locationPrefix = _conf.GetValue<string>("NfsLocation");
        var selectedImage = await _image.ReadImage(id);
        var img = System.IO.File.OpenRead(locationPrefix + selectedImage.ImageUrl);

        return File(img, "image/jpg");
    }

    [HttpGet("getImagesByCategory/{categoryId}")]
    public async Task<ActionResult<ImageDto[]>> GetImagesByCategory(int categoryId)
    {
        var plImages = await _image.GetImagesByCategory(categoryId);
        if (plImages == null || plImages.Length == 0)
        {
            return NotFound("No images found for this category");
        }
        return Ok(plImages);
    }

    [HttpPost("getFilesForThisUser")]
    public async Task<ActionResult<ImageDto[]>> GetImagesByUser([FromBody] CategoryParams ip)
    {
        var plImages = await _image.findImagesByUser(ip);
        if (plImages == null || plImages.Length == 0)
        {
            return NotFound("No images found for this user");
        }
        return Ok(plImages);
    }

    [HttpPost("addImage")]
    public async Task<ActionResult<string>> AddImage([FromBody] ImageDto imagedto)
    {
        var result = await _image.createImage(imagedto);
        if (await _image.SaveChangesAsync())
        {
            return Ok("Image added");
        }
        return BadRequest("Image was not added");
    }
    
    [HttpDelete("deleteImage/{id}")]
    public async Task<ActionResult<int>> DeleteImage(int id)
    {
        var result = await _image.deleteImage(id);
        if (await _image.SaveChangesAsync())
        {
            return Ok("Image removed");
        }
        return BadRequest("Image was not deleted");
    }

    [HttpPut("updateImage")]
    public async Task<ActionResult<int>> UpdateImage(ImageDto imagedto)
    {
        var result = await _image.updateImage(imagedto);
        if (await _image.SaveChangesAsync())
        {
            return Ok("Image updated");
        }
        return BadRequest("Image was not updated");
    }

    [HttpGet("findImage/{Id}", Name = "GetImage")]
    public async Task<ActionResult<ImageDto>> FindImage(int Id)
    {
        return await _image.ReadImage(Id);
    }

   [HttpGet("getCarousel/{Id}")]
    public async Task<ActionResult<CarouselDto>> getCarousel(int Id)
    {
        return await _image.getCarouselData(Id);
    }
}
